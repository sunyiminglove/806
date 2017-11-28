using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Modbus.Device;
using MySql.Data.MySqlClient;
//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Structure;

namespace Modustest
{
    /// <summary>
    /// Description of Form1.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// 数据变量
        /// </summary>
        #region
        Thread th;
        ModbusSerialMaster msm;

        ushort AGV_AskSingle1 = 0;//AGV请求信号
        ushort AGV_AskSingle2 = 0;//AGV请求信号
        ushort AGV_In1 = 0;//AGV在1号设备里 在：1 不在：0
        ushort AGV_In2 = 0;//AGV在2号设备里
        ushort AGV_In3 = 0;//AGV在3号设备里
        ushort AGV_In4 = 0;//AGV在4号设备里
	
	    ushort adress_qingqiujinru = 22;//AGV请求进入地址
	    ushort adress_AGVjinru = 20;//AGV进入设备区域地址
	    ushort adress_yunxujinru = 10;//允许AGV进入地址
	    ushort adress_qidong = 9;//AGV启动信号地址
	    public static  ushort[] AGV_Start=new ushort[4];//启动信号数组

        //byte SlaveAdress = 2;
        //ushort RegisterAdress = 0;

        public static ushort[] PlcDataTemplete = new ushort[30];//临时


        public static ushort[] PlcData1 = new ushort[30];//叉车1
        public static ushort[] PlcData2 = new ushort[30];//叉车2
        public static ushort[] PlcData3 = new ushort[20];//电梯
        public static ushort[] PlcData6 = new ushort[100];//modbus调试

        public static ushort[] PlcData11 = new ushort[20];//设备11 
        public static ushort[] PlcData12 = new ushort[20];//设备12
        public static ushort[] PlcData13 = new ushort[20];//设备13
        public static ushort[] PlcData14 = new ushort[20];//设备14

        bool ListenState = false;

        #endregion

        string mapFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Map\\map.jpg";

        Int32 TotalNum = 0;//站点总数

        double scaleWidth = 0;//界面比例
        double scaleHeight = 0;//界面比例

        byte AGV1_adress = 2;
        byte AGV2_adress = 3;
        //创建数据库连接对象
        MySqlConnection mysql;

        /// <summary>
        /// 建立mysql数据库链接
        /// </summary>
        /// <returns></returns>
        private static MySqlConnection MySqlCon(string adress, string id, string password)
        {
            String mysqlStr = "Database=agv;Data Source=" + adress + ";User Id=" + id + ";Password=" + password + ";pooling=false;CharSet=utf8;port=3306";
            MySqlConnection mysql = new MySqlConnection(mysqlStr);
            return mysql;
        }
        /// <summary>
        /// 建立执行命令语句对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="mysql"></param>
        /// <returns></returns>
        private static MySqlCommand getSqlCommand(String sql, MySqlConnection mysql)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mysql);
            //  MySqlCommand mySqlCommand = new MySqlCommand(sql);
            // mySqlCommand.Connection = mysql;
            return mySqlCommand;
        }

        public struct station
        {
            public string id;
            public string num;
            public string size;
            public string wordx;
            public string wordy;
            public string rectx;
            public string recty;
        };

        /// <summary>
        /// 用户添加数据
        /// </summary>
        /// <param name="sta">站点信息结构体</param>
        private void userAdd(station sta)
        {
            try
            {
                //插入sql
                String sqladd = "INSERT INTO `station` (`userid`, `num`, `size`, `wordx`, `wordy`, `rectx`, `recty`) VALUES ('" + sta.id + "','" + sta.num + "','" + sta.size + "','" + sta.wordx + "','" + sta.wordy + "','" + sta.rectx + "','" + sta.recty + "')";
                MySqlCommand mySqlCommand = getSqlCommand(sqladd, mysql);
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        private void userDel(string id)
        {
            //删除sql
            String sqldel = "delete from station where userid = '" + id + "'";
            MySqlCommand mySqlCommand = getSqlCommand(sqldel, mysql);
            mySqlCommand.ExecuteNonQuery();
        }


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sta">站点信息结构体</param>
        private void userUpadta(station sta)
        {
            //修改sql
            String sqlUpdate = "update station set num='" + sta.num + "',size='" + sta.size + "',wordx='" + sta.wordx + "',wordy='" + sta.wordy + "', rectx='" + sta.rectx + "' ,recty='" + sta.recty + "'where userid='" + sta.id + "'";
            MySqlCommand mySqlCommand = getSqlCommand(sqlUpdate, mysql);
            mySqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// 用户查询数据
        /// </summary>
        /// <param name="id">id号</param>
        public station userfindById(string id)
        {
            station sta = new station();
            try
            {
                //查询数据
                String sqlSearch = "select * from station where userid='" + id + "' limit 0,1";
                MySqlCommand mySqlCommand = getSqlCommand(sqlSearch, mysql);

                MySqlDataReader reader = mySqlCommand.ExecuteReader();

                if (reader.Read())//存在，更新数据，不存在不处理
                {
                    if (reader.HasRows)
                    {
                        sta.id = reader.GetString(1);
                        sta.num = reader.GetString(2);
                        sta.size = reader.GetString(3);
                        sta.wordx = reader.GetString(4);
                        sta.wordy = reader.GetString(5);
                        sta.rectx = reader.GetString(6);
                        sta.recty = reader.GetString(7);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return sta;
        }


        /// <summary>
        /// 用户查询数据
        /// </summary>
        /// <param name="num">地标号</param>
        public station userfindbyNum(string num)
        {
            station sta = new station();
            try
            {
                //查询数据
                String sqlSearch = "select * from station where num='" + num + "' limit 0,1";
                MySqlCommand mySqlCommand = getSqlCommand(sqlSearch, mysql);

                MySqlDataReader reader = mySqlCommand.ExecuteReader();

                if (reader.Read())//存在，更新数据，不存在不处理
                {
                    if (reader.HasRows)
                    {
                        sta.id = reader.GetString(1);
                        sta.num = reader.GetString(2);
                        sta.size = reader.GetString(3);
                        sta.wordx = reader.GetString(4);
                        sta.wordy = reader.GetString(5);
                        sta.rectx = reader.GetString(6);
                        sta.recty = reader.GetString(7);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return sta;
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        public int userGetNum()
        {
            int num = 0;
            try
            {
                //查询数据
                String sqlcount = "select count(*) from station";
                MySqlCommand mySqlCommand = getSqlCommand(sqlcount, mysql);
                num = Convert.ToInt32(mySqlCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
            return num;
        }

        /// <summary>
        /// 读取所有数据
        /// </summary>
        public station[] userGetAll()
        {
            int num = userGetNum();
            station[] sta = new Form1.station[num];
            try
            {
                //查询数据
                String sqlSearch = "select * from station order by `userid` ASC";
                MySqlCommand mySqlCommand = getSqlCommand(sqlSearch, mysql);

                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                int i = 0;
                while (reader.Read())//存在，更新数据
                {
                    sta[i].id = reader.GetString(1);
                    sta[i].num = reader.GetString(2);
                    sta[i].size = reader.GetString(3);
                    sta[i].wordx = reader.GetString(4);
                    sta[i].wordy = reader.GetString(5);
                    sta[i].rectx = reader.GetString(6);
                    sta[i].recty = reader.GetString(7);
                    i++;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return sta;
        }

        Image bitmap;
        public Form1()
        {
            InitializeComponent();

            //读取地图
            Image img = Image.FromFile(mapFileName);
            bitmap = new Bitmap(img);
            img.Dispose();

            //配置文件路径
            //			Ini.Filename = "\\Map\\MapInfo.ini";
            //			if (Ini.Read("站点总数", "站点数") != "无法读取！" && Ini.Read("站点总数", "站点数") != "0")
            //			{
            //				TotalNum = int.Parse(Ini.Read("站点总数", "站点数"));
            //				pictureBox1.Image = bitmap;
            //				NowNum = 1;
            //			}

            //禁止检测跨线程访问
            Control.CheckForIllegalCrossThreadCalls = false;

            //添加数据列表
            //列宽
            dataGridView1.Columns[0].Width = 120;
            dataGridView1.Columns[1].Width = 120;
            for (int i = 0; i < 100; i++)
            {
                dataGridView1.Rows.Add(i.ToString());
                //行高
                dataGridView1.Rows[i].Height = 25;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            mysql = MySqlCon("localhost", "root", "root");
            //打开数据库
            try
            {
                mysql.Open();
            }
            catch
            {
            }
            
            #region //判断配置文件夹是否存在，不存在自动创建
            //系统启动增加文件夹，存放地图信息
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Map") == false)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Map");
            }
            //系统启动增加文件夹，存放路径信息
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Road") == false)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Road");
            }
            //系统启动增加文件夹，存放地图系统配置信息
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\System") == false)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\System");
            }
            #endregion

            scaleWidth = pictureBox1.Width / (float)(pictureBox2.Width);
            scaleHeight = pictureBox1.Size.Height / (float)(pictureBox2.Height);
            //隐藏标签
            tabControl1.Region = new Region(new RectangleF(tabPage1.Left, tabPage1.Top, tabPage1.Width, tabPage1.Height));
            tabControl1.BackColor = Color.White;

            #region //地图编辑
            //查询本地是否有地图图片
            if (File.Exists(mapFileName))
            {
                //存在
                //显示已经设置的站点
                updateToPic2();
            }
            else
            {
                //不存在
                MessageBox.Show("系统没有地图信息，请加载路线地图！");
            }
            #endregion

            tabControl1.SelectTab(2);
            //自动启动
            button_start.PerformClick();

            tabControl1.SelectTab(0);

            pictureBox2.BackgroundImage = bitmap;
            //            tabControl1.SelectTab(5);


            //禁止电梯操作
            buttonylhj.Enabled = false;
            buttonslhj.Enabled = false;
            buttonqyl.Enabled = false;
            buttonqsl.Enabled = false;
            buttonkm.Enabled = false;
            buttongm.Enabled = false;
            
            //机械手
            robot1.DeviceName = "卸货区";
            robot1.DeviceID = 11;
            robot2.DeviceName = "入库区";
            robot2.DeviceID = 12;
            robot3.DeviceName = "三楼区";
            robot3.DeviceID = 13;
            robot4.DeviceName = "粉碎区";
            robot4.DeviceID = 14;
            
            agvSensor1.AgvNumber = 1;
            agvSensor1.AgvID = 2;
            agvSensor2.AgvNumber = 2;
            agvSensor2.AgvID = 3;
      }

        #region 调试界面
        private void Button_startClick(object sender, EventArgs e)
        {
            try
            {
                if (button_start.Text == "启动")
                {
                    button_start.Text = "停止";
                    serialPort1.PortName = SerialPortList.Text.Trim();
                    serialPort1.Open();
                    msm = ModbusSerialMaster.CreateRtu(serialPort1);
                    msm.Transport.WriteTimeout = int.Parse(textBox11.Text);//写超时时间，单位ms,-1为一直等待
                    msm.Transport.ReadTimeout = int.Parse(textBox11.Text);//读超时时间，单位ms，-1为一直等待
                    msm.Transport.Retries = 2;//重试次数
                    msm.Transport.WaitToRetryMilliseconds = 1;//等待重试时间，单位ms

                    msg("串口已打开！");
                    th = new Thread(Listen);
                    ListenState = true;
                    th.Start();
                    msg("开始轮询！");
                }
                else
                {
                    button_start.Text = "启动";
                    ListenState = false;
                    Thread.Sleep(300);
                    serialPort1.Close();

                }
            }
            catch 
            {
                msg("启动失败！");
                //textBox2.Text = ex.ToString();
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("通信超时！");
            }
        }
        
		//定时扫描设备
     	
        void Listen()
        {
            //电梯参数初始化
            PlcData3[13] = 1;
            PlcData3[14] = 1;
            PlcData3[15] = 1;
            PlcData3[16] = 1;
            PlcData3[17] = 1;
            PlcData3[19] = 1;
            //int i = 0;
            while (ListenState)
            {
                try
                {
                    Thread.Sleep(10);
                    
                    byte deviceidTemp = 0;//设备ID临时变量

                    #region 叉车1
                    try
                    {
                    	deviceidTemp = 2;
                        if (agvSensor1.Checked)
                        {
                            //读取
                            PlcData1 = msm.ReadHoldingRegisters(deviceidTemp, 0, 30);

                            //地标
                            textBoxdb1.Text = PlcData1[0].ToString();
                            //磁导航信息显示
                            label11.Text = ToErJin(PlcData1[1], 16);
                            label12.Text = ToErJin(PlcData1[2], 8);
                            label14.Text = ToErJin(PlcData1[3], 8);

                            //运行
                            if (PlcData1[4] == 1) labelyx.ForeColor = Color.Green;
                            else
                                labelyx.ForeColor = Color.Red;

                            //出轨前
                            if (PlcData1[5] == 0)
                            {
                                labelcgq.Text = "●";
                                labelcgq.ForeColor = Color.Green;
                            }
                            else
                            {
                                labelcgq.Text = "● 出轨！";
                                labelcgq.ForeColor = Color.Red;
                            }
                            //出轨后左
                            if (PlcData1[6] == 0)
                            {
                                labelcghz.Text = "●";
                                labelcghz.ForeColor = Color.Green;
                            }
                            else
                            {
                                labelcghz.Text = "● 出轨！";
                                labelcghz.ForeColor = Color.Red;
                            }
                            //出轨后右
                            if (PlcData1[7] == 0)
                            {
                                labelcghy.Text = "●";
                                labelcghy.ForeColor = Color.Green;
                            }
                            else
                            {
                                labelcghy.Text = "● 出轨！";
                                labelcghy.ForeColor = Color.Red;
                            }
                            //模式
                            if (PlcData1[8] == 0) labelmode.Text = "手动"; else labelmode.Text = "自动";

                            //速度
                            labelspeed1.Text = PlcData1[9].ToString();
							
                            
                            //左叉臂避障
                            agvSensor1.ZuoChaBiBiZhang=PlcData1[10];
                 			
                            //右叉臂避障
                            agvSensor1.YouChaBiBiZhang=PlcData1[11];
                            
                            //左叉臂检测
                            agvSensor1.ZuoChaBiJianCe=PlcData1[12];
                            
                            //右叉臂检测
                            agvSensor1.YouChaBiJianCe=PlcData1[13];
                            
                            //远避障
                            agvSensor1.YuanChengBiZhang=PlcData1[14];
                           
                            //近避障
                            agvSensor1.JinChengBiZhang=PlcData1[15];
                            
                            //机械防撞
                            agvSensor1.JiXieBiZhang=PlcData1[16];

                            
                            if (radioButtonAGV1.Checked)
                            {
                                //18 三楼外呼
                                PlcData3[15] = PlcData1[18];
                                //19 一楼外呼
                                PlcData3[16] = PlcData1[19];
                                //20 3按钮
                                PlcData3[17] = PlcData1[20];
                                //21 1按钮
                                PlcData3[19] = PlcData1[21];
                                //22 开门
                                if (PlcData1[22] == 10)
                                {
                                    PlcData3[14] = 0;
                                }
                                else
                                    PlcData3[14] = PlcData1[22];
                                //23 关门
                                PlcData3[13] = PlcData1[23];
                            }

                            //24 三楼平层
                            PlcData1[24] = PlcData3[7];
                            //25 一楼平层
                            PlcData1[25] = PlcData3[9];

                            //26 三楼门状态
                            if (PlcData3[4] == 1)
                                PlcData1[26] = 0;
                            else
                                PlcData1[26] = 1;

                            //27 一楼门状态
                            if (PlcData3[6] == 1)
                                PlcData1[27] = 0;
                            else
                                PlcData1[27] = 1;

                            //路径号
                            labellj1.Text = PlcData1[28].ToString();

                            //清空叉车呼叫信号
                            PlcData1[18] = 1;
                            PlcData1[19] = 1;
                            PlcData1[20] = 1;
                            PlcData1[21] = 1;

                            if (PlcData1[22] != 10)
                            {
                                PlcData1[22] = 1;
                            }
                            
                            PlcData1[23] = 1;

                            //单次发送不要超过20字节
                            ushort[] PlcDatatemp = new ushort[10];
                            for (int i = 0; i < 10; i++)
                            {
                                PlcDatatemp[i] = PlcData1[i + 18];
                            }

                            msm.WriteMultipleRegisters(deviceidTemp, 18, PlcDatatemp);

                            //报告设备在线
                            msm.WriteSingleRegister(deviceidTemp, 37, 1);
							
                       
                        	//读取32，AGV请求进入设备
                            PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, 32, 1);
                            AGV_AskSingle1 = PlcDataTemplete[0];

                            //读取AGV在哪个设备里面
                            PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, 38, 4);
                            AGV_In1 = PlcDataTemplete[0];
                            AGV_In2 = PlcDataTemplete[1];
                            AGV_In3 = PlcDataTemplete[2];

                            robot1.JinRuGongZuoQu = AGV_In1;
                            robot2.JinRuGongZuoQu = AGV_In2;
                            robot3.JinRuGongZuoQu = AGV_In3;
                           
                            //给AGV写入设备发送的启动信号
                            msm.WriteMultipleRegisters(deviceidTemp, 42, AGV_Start);
                            
                            
                            agvSensor1.AgvOnline = true;

                            labelch1.BackColor = Color.YellowGreen;
                        }
                        else
                        {
                            agvSensor1.AgvOnline = false;
	                        labelch1.BackColor = Color.LightCoral;
                        }
                    }
                    catch
                    {

                        labelch1.BackColor = Color.LightCoral;
						agvSensor1.AgvOnline = false;
						agvSensor1.Checked = false;
                        msg("叉车1读取响应超时！");
                    }
                    #endregion

                    #region 叉车2
                    try
                    {
                    	deviceidTemp = 3;
                        if (agvSensor2.Checked)
                        {
                            PlcData2 = msm.ReadHoldingRegisters(deviceidTemp, 0, 30);

                            //地标
                            textBoxdb2.Text = PlcData2[0].ToString();
                            //磁导航信息显示
                            labelqdh2.Text = ToErJin(PlcData2[1], 16);
                            labelzcb2.Text = ToErJin(PlcData2[2], 8);
                            labelycb2.Text = ToErJin(PlcData2[3], 8);

                            //运行
                            if (PlcData2[4] == 1) labelyx2.ForeColor = Color.Green;
                            else
                                labelyx2.ForeColor = Color.Red;

                            //前出轨
                            if (PlcData2[5] == 0)
                            {
                                labelqdhcg2.Text = "●";
                                labelqdhcg2.ForeColor = Color.Green;
                            }
                            else
                            {
                                labelqdhcg2.Text = "● 出轨！";
                                labelqdhcg2.ForeColor = Color.Red;
                            }
                            //左后出轨
                            if (PlcData2[6] == 0)
                            {
                                labelzcbcg2.Text = "●";
                                labelzcbcg2.ForeColor = Color.Green;
                            }
                            else
                            {
                                labelzcbcg2.Text = "● 出轨！";
                                labelzcbcg2.ForeColor = Color.Red;
                            }
                            //右后出轨
                            if (PlcData2[7] == 0)
                            {
                                labelycbcg2.Text = "●";
                                labelycbcg2.ForeColor = Color.Green;
                            }
                            else
                            {
                                labelycbcg2.Text = "● 出轨！";
                                labelycbcg2.ForeColor = Color.Red;
                            }
                            //模式
                            if (PlcData2[8] == 0) labelms2.Text = "手动"; else labelms2.Text = "自动";

                            //速度
                            labelspeed2.Text = PlcData2[9].ToString();

                            //左叉臂避障
                            agvSensor2.ZuoChaBiBiZhang=PlcData2[10];
                 			
                            //右叉臂避障
                            agvSensor2.YouChaBiBiZhang=PlcData2[11];
                            
                            //左叉臂检测
                            agvSensor2.ZuoChaBiJianCe=PlcData2[12];
                            
                            //右叉臂检测
                            agvSensor2.YouChaBiJianCe=PlcData2[13];
                            
                            //远避障
                            agvSensor2.YuanChengBiZhang=PlcData2[14];
                           
                            //近避障
                            agvSensor2.JinChengBiZhang=PlcData2[15];
                            
                            //机械防撞
                            agvSensor2.JiXieBiZhang=PlcData2[16];


                            if (radioButtonAGV2.Checked)
                            {
                                //18 三楼外呼
                                PlcData3[15] = PlcData2[18];
                                //19 一楼外呼
                                PlcData3[16] = PlcData2[19];
                                //20 3按钮
                                PlcData3[17] = PlcData2[20];
                                //21 1按钮
                                PlcData3[19] = PlcData2[21];
                                //22 开门
                                if (PlcData2[22] == 10)
                                {
                                    PlcData3[14] = 0;
                                }
                                else
                                    PlcData3[14] = PlcData2[22];

                                //23 关门
                                PlcData3[13] = PlcData2[23];
                            }

                            //24 三楼平层
                            PlcData2[24] = PlcData3[7];
                            //25 一楼平层
                            PlcData2[25] = PlcData3[9];

                            //26 三楼门状态
                            if (PlcData3[4] == 1)
                                PlcData2[26] = 0;
                            else
                                PlcData2[26] = 1;

                            //27 一楼门状态
                            if (PlcData3[6] == 1)
                                PlcData2[27] = 0;
                            else
                                PlcData2[27] = 1;

                            //路径号
                            labellj2.Text = PlcData2[28].ToString();


                            //清空叉车呼叫信号
                            PlcData2[18] = 1;
                            PlcData2[19] = 1;
                            PlcData2[20] = 1;
                            PlcData2[21] = 1;

                            if (PlcData2[22] != 10)
                            {
                                PlcData2[22] = 1;
                            }
                            
                            PlcData2[23] = 1;

                            //单次发送不要超过20字节
                            ushort[] PlcDatatemp = new ushort[10];
                            for (int i = 0; i < 10; i++)
                            {
                                PlcDatatemp[i] = PlcData2[i + 18];
                            }

                            msm.WriteMultipleRegisters(deviceidTemp, 18, PlcDatatemp);

                            
                            //报告设备在线
                            msm.WriteSingleRegister(deviceidTemp, 37, 1);
                            
                           
                        	//读取32，AGV请求进入设备
                            PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, 32, 1);
                            AGV_AskSingle2 = PlcDataTemplete[0];

                            //读取AGV在哪个设备里面
                            PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, 38, 4);
                            AGV_In4 = PlcDataTemplete[3];

                            robot4.JinRuGongZuoQu = AGV_In4;
                            
                            //给AGV写入设备发送的启动信号
                            msm.WriteMultipleRegisters(deviceidTemp, 42, AGV_Start);
                           
                            labelch2.BackColor = Color.YellowGreen;
                            agvSensor2.AgvOnline = true;
                        }
                        else
                        {
                            agvSensor2.AgvOnline = false;
	                        labelch2.BackColor = Color.LightCoral;
                        }
                    }
                    catch
                    {

                        labelch2.BackColor = Color.LightCoral;
						agvSensor2.AgvOnline = false;
						agvSensor2.Checked = false;
                        msg("叉车2读取响应超时！");
                    }
                    #endregion

                    #region 电梯
                    try
                    {
                    	deviceidTemp = 4;
                        if (checkBox3.Checked)
                        {
                            ushort[] PlcDatatemp;
                            //读取
                            PlcDatatemp = msm.ReadHoldingRegisters(deviceidTemp, 0, 10);

                            for (int i = 0; i < 10; i++)
                            {
                                PlcData3[i] = PlcDatatemp[i];
                            }


                            //一楼平层
                            if (PlcData3[9] == 0)
                            {
                                labelylzs.BackColor = Color.YellowGreen;
                            }
                            else
                            {
                                labelylzs.BackColor = Color.LightCoral;
                            }
                            //三楼平层
                            if (PlcData3[7] == 0)
                            {
                                labelslzs.BackColor = Color.YellowGreen;
                            }
                            else
                            {
                                labelslzs.BackColor = Color.LightCoral;
                            }

                            //一楼门状态
                            if (PlcData3[5] == 0)//关到位
                            {
                                buttonylzm.Visible = true;
                                buttonylym.Visible = true;
                            }
                            else if (PlcData3[6] == 0)//开到位
                            {
                                buttonylzm.Visible = false;
                                buttonylym.Visible = false;
                            }
                            //三楼门状态
                            if (PlcData3[3] == 0)//关到位
                            {
                                buttonslzm.Visible = true;
                                buttonslym.Visible = true;
                            }
                            else if (PlcData3[4] == 0)//开到位
                            {
                                buttonslzm.Visible = false;
                                buttonslym.Visible = false;
                            }

                            //写入
                            msm.WriteMultipleRegisters(deviceidTemp, 0, PlcData3);
                            
                            //如果有按电梯按钮，延时1s
                            if (PlcData3[13]!=1|| PlcData3[14] != 1 || PlcData3[15] != 1 || PlcData3[16] != 1 || PlcData3[17] != 1 || PlcData3[19] != 1 )
                            {
                                Thread.Sleep(1000); 
                            }
                            
                            //清空电梯按钮信号
                            PlcData3[13] = 1;
                            PlcData3[14] = 1;
                            PlcData3[15] = 1;
                            PlcData3[16] = 1;
                            PlcData3[17] = 1;
                            PlcData3[19] = 1;
                            
                            buttondtzx.BackColor = Color.YellowGreen;
                            buttondtzx.Text = "在线";
                        }
                    }
                    catch
                    {
                        buttondtzx.BackColor = Color.LightCoral;
                        buttondtzx.Text = "离线";
                        checkBox3.Checked =false;
                        msg("电梯读取响应超时！");
                    }
                    #endregion

                    #region modbus工具
                    try
                    {
                        if (checkBox6.Checked)
                        {

                            PlcData6 = msm.ReadHoldingRegisters(byte.Parse(textBoxStation6.Text.Trim()), 0, 30);
                            int i = 0;
                            foreach (var item in PlcData6)
                            {
                                dataGridView1.Rows[i].Cells[1].Value = item.ToString();
                                i++;
                            }
                        }

                    }
                    catch 
                    {
                        //						msg(ex.ToString());
                        msg("modbus工具响应超时！");
                    }
                    #endregion

                    #region 设备1
                    if (robot1.Checked)
                    {
                        try
                        {
                            deviceidTemp = 11;
                            //AGV请求进入某个设备区域
                            if (AGV_AskSingle1 == 1)
                            {
                                //给相应设备发送请求信号
                                msm.WriteSingleRegister(deviceidTemp, adress_qingqiujinru, 1);
                                //读取准入信号
                                PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, adress_yunxujinru, 1);
                                
                                robot1.YunXuJinRu = PlcDataTemplete[0];
                                
                                //将准入信号转发给AGV
                                if (agvSensor1.AgvOnline) {
                                	  try {
                                	 msm.WriteSingleRegister(AGV1_adress, 33, PlcDataTemplete[0]);
                                } 
                                catch {
                                	msg("AGV1不在线！");
                                }
                                }
                              

                                //告诉设备AGV当前是否在设备里面
                                msm.WriteSingleRegister(deviceidTemp, adress_AGVjinru, AGV_In1);
                                
                                robot1.QingQiuJinRu = 1;
                            }
                            else
                            {
                                //给设备清空请求
                                msm.WriteSingleRegister(deviceidTemp, adress_qingqiujinru, 0);
                                //清空准入信号转发给AGV
                                if (agvSensor1.AgvOnline) {
                                	 try {
                                	 msm.WriteSingleRegister(AGV1_adress, 33, 0);
                                } 
                                catch {
                                	msg("AGV1不在线！");
                                }
                                }
                               
                               
                                robot1.YunXuJinRu = 0;
                                
                                //告诉设备AGV当前是否在设备里面
                                msm.WriteSingleRegister(deviceidTemp, adress_AGVjinru, AGV_In1);
                                robot1.QingQiuJinRu = 0;
                            }
                            
                            //读取设备的启动信号
                            PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, adress_qidong, 1);
                            AGV_Start[0]=PlcDataTemplete[0];
                            robot1.QiDong = AGV_Start[0];
                            
                            //在线
                            robot1.DeviceOnline = true;
                        }
                        catch
                        {
                        	robot1.DeviceOnline = false;
                        	robot1.Checked=false;
                            //离线
                            msg("设备" + deviceidTemp.ToString() + "响应超时！");
                        }
                    }
                    #endregion

                    #region 设备2
                    if (robot2.Checked)
                    {
                        try
                        {
                            deviceidTemp = 12;
                            //AGV请求进入某个设备区域
                            if (AGV_AskSingle1 == 2)
                            {
                                //给相应设备发送请求信号
                                msm.WriteSingleRegister(deviceidTemp, adress_qingqiujinru, 1);
                                //读取准入信号
                                PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, adress_yunxujinru, 1);
                                
                                robot2.YunXuJinRu = PlcDataTemplete[0];
                                
                                //将准入信号转发给AGV
                                if (agvSensor1.AgvOnline) {
                                	try {
                                	msm.WriteSingleRegister(AGV1_adress, 34, PlcDataTemplete[0]);
                                } 
                                catch {
                                	msg("AGV1不在线！");
                                }
                                }
                                 
                                

                                //告诉设备AGV当前是否在设备里面
                                msm.WriteSingleRegister(deviceidTemp, adress_AGVjinru, AGV_In2);
                                
                                  robot2.QingQiuJinRu = 1;
                            }
                            else
                            {
                                //给设备清空请求
                                msm.WriteSingleRegister(deviceidTemp, adress_qingqiujinru, 0);
                                //清空准入信号转发给AGV
                                if (agvSensor1.AgvOnline) {
                                	 try {
                                	 msm.WriteSingleRegister(AGV1_adress, 34, 0);
                                } 
                                catch {
                                	msg("AGV1不在线！");
                                }
                                }
                                
                               
                                
                                robot2.YunXuJinRu = 0;

                                //告诉设备AGV当前是否在设备里面
                                msm.WriteSingleRegister(deviceidTemp, adress_AGVjinru, AGV_In2);
                                 robot2.QingQiuJinRu = 0;
                            }
                            
                            //读取设备的启动信号
                            PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, adress_qidong, 1);
                            AGV_Start[1]=PlcDataTemplete[0];
                            robot2.QiDong = AGV_Start[1];
                            
                            //在线
                            robot2.DeviceOnline = true;
                        }
                        catch
                        {
                        	robot2.DeviceOnline = false;
                        	robot2.Checked=false;
                            //离线
                            msg("设备" + deviceidTemp.ToString() + "响应超时！");
                        }
                    }
                    #endregion

                    #region 设备3
                    if (robot3.Checked)
                    {
                        try
                        {
                            deviceidTemp = 13;
                            //AGV请求进入某个设备区域
                            if (AGV_AskSingle1 == 3)
                            {
                                //给相应设备发送请求信号
                                msm.WriteSingleRegister(deviceidTemp, adress_qingqiujinru, 1);
                                //读取准入信号
                                PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, adress_yunxujinru, 1);
                                
                                robot3.YunXuJinRu = PlcDataTemplete[0];
                                
                                //将准入信号转发给AGV
                                if (agvSensor1.AgvOnline) {
                                try {
                                	 msm.WriteSingleRegister(AGV1_adress, 35, PlcDataTemplete[0]);
                                } 
                                catch {
                                	msg("AGV1不在线！");
                                }
                                }

                               

                                //告诉设备AGV当前是否在设备里面
                                msm.WriteSingleRegister(deviceidTemp, adress_AGVjinru, AGV_In3);
                                 robot3.QingQiuJinRu = 1;
                            }
                            else
                            {
                                //给设备清空请求
                                msm.WriteSingleRegister(deviceidTemp, adress_qingqiujinru, 0);
                                //清空准入信号转发给AGV
                                if (agvSensor1.AgvOnline) {
                                	try {
                                	 msm.WriteSingleRegister(AGV1_adress, 35, 0);
                                } 
                                catch {
                                	msg("AGV1不在线！");
                                }
                                }
                                 
                               
                                robot3.YunXuJinRu = 0;

                                //告诉设备AGV当前是否在设备里面
                                msm.WriteSingleRegister(deviceidTemp, adress_AGVjinru, AGV_In3);
                                 robot3.QingQiuJinRu = 0;
                            }
                            
                            //读取设备的启动信号
                            PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, adress_qidong, 1);
                            AGV_Start[2]=PlcDataTemplete[0];
                            robot3.QiDong = AGV_Start[2];
                            
                            //在线
                            robot3.DeviceOnline = true;
                        }
                        catch
                        {
                        	robot3.DeviceOnline = false;
                        	robot3.Checked=false;
                            //离线
                            msg("设备" + deviceidTemp.ToString() + "响应超时！");
                        }
                    }
                    #endregion

                    #region 设备4
                    if (robot4.Checked)
                    {
                        try
                        {
                            deviceidTemp = 14;
                            //AGV请求进入某个设备区域
                            if (AGV_AskSingle2 == 4)
                            {
                                //给相应设备发送请求信号
                                msm.WriteSingleRegister(deviceidTemp, adress_qingqiujinru, 1);
                                //读取准入信号
                                PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, adress_yunxujinru, 1);
                                
                                robot4.YunXuJinRu = PlcDataTemplete[0];
                                
                                //将准入信号转发给AGV
                                if (agvSensor2.AgvOnline) {
                                	try {
                                	 msm.WriteSingleRegister(AGV2_adress, 36, PlcDataTemplete[0]);
                                } 
                                catch {
                                	msg("AGV2不在线！");
                                }
                                }
                                //告诉设备AGV当前是否在设备里面
                                msm.WriteSingleRegister(deviceidTemp, adress_AGVjinru, AGV_In4);
                                 robot4.QingQiuJinRu = 1;
                            }
                            else
                            {
                                //给设备清空请求
                                msm.WriteSingleRegister(deviceidTemp, adress_qingqiujinru, 0);
                                //清空准入信号转发给AGV
                                if (agvSensor2.AgvOnline) {
                                	try {
                                	msm.WriteSingleRegister(AGV2_adress, 36, 0);
                                } 
                                catch {
                                	msg("AGV2不在线！");
                                }
                                }
                                 
                                
                                 robot4.YunXuJinRu = 0;
                                //告诉设备AGV当前是否在设备里面
                                msm.WriteSingleRegister(deviceidTemp, adress_AGVjinru, AGV_In4);
                                 robot4.QingQiuJinRu = 0;
                            }
                            
                            //读取设备的启动信号
                            PlcDataTemplete = msm.ReadHoldingRegisters(deviceidTemp, adress_qidong, 1);
                            AGV_Start[3]=PlcDataTemplete[0];
                            robot4.QiDong = AGV_Start[3];
                            
                            //在线
                            robot4.DeviceOnline = true;
                        }
                        catch
                        {
                        	robot4.DeviceOnline = false;
                        	robot4.Checked=false;
                            //离线
                            msg("设备" + deviceidTemp.ToString() + "响应超时！");
                        }
                    }
                    #endregion

                    labelRead.ForeColor = Color.Green;
                    Thread.Sleep(20);
                    labelRead.ForeColor = Color.Red;
                }
                catch
                {
                    try
                    {
                        msg("监听响应超时！");
                    }
                    catch
                    { }
                }


            }
        }

        private void DataGridView1CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                msm.WriteSingleRegister(byte.Parse(textBoxStation6.Text.Trim()), (ushort)(e.RowIndex), ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()));
                labelWrite.ForeColor = Color.Green;
                Thread.Sleep(20);
                labelWrite.ForeColor = Color.Red;
            }
            catch
            {
                msg("写入数据超时！");
                //textBox2.Text = ex.ToString();
                // MessageBox.Show("通信超时！");
            }
        }

        /// <summary>
        /// 转换为二进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToErJin(int value, int length)
        {
            int num = value; // 十进制
            char[] b = new char[100];
            string result = string.Empty; // 二进制
            int i = 0;
            while (num != 0)
            {
                b[i] = Convert.ToChar((num % 2).ToString());
                num = num / 2;
                ++i;
            }
            while (i > 0)
            {
                i--;
                result += b[i];
            }

            result = result.Replace("1", "○");
            result = result.Replace("0", "●");
            int num1 = length - result.Length;
            for (i = 0; i < num1; i++)
            {
                result = "●" + result;
            }

            return result;
        }

        //清空显示
        private void Button7Click(object sender, EventArgs e)
        {
            textBox3.Text = string.Empty;
            labelerr.Text = "0";
        }

        int errnum;
        private void msg(string str)
        {
            errnum++;
            textBox3.AppendText(str + "\r\n");  //添加文本
            textBox3.ScrollToCaret();    //自动显示至最后行
            labelerr.Text = errnum.ToString();
        }

        private void TextBoxStation6TextChanged(object sender, EventArgs e)
        {
            //判断输入站号是否为数字
            if (Regex.Match(textBoxStation6.Text, "^\\d+$").Success)
            {
                textBoxStation6.BackColor = Color.White;
            }
            else
            {
                textBoxStation6.BackColor = Color.Red;
            }
        }
        #endregion

        #region 运行界面

        //更新站点设置信息到图片
        private void updateToPic()
        {
            try
            {
                Graphics gra = this.pictureBox1.CreateGraphics();
                gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                gra.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                // 作为演示，我们用Arial字体，大小为32，红色。
                FontFamily fm = new FontFamily("Arial");
                Font font = new Font(fm, 32, FontStyle.Regular, GraphicsUnit.Pixel);

                TotalNum = userGetNum();
                //读取站点信息
                if (TotalNum > 0)
                {
                    Color color = Color.Red;

                    station[] sta = new Form1.station[TotalNum];
                    sta = userGetAll();
                    for (Int16 i = 0; i < TotalNum; i++)
                    {
                        if ((sta[i].num == NowNum1.ToString()) || (sta[i].num == NowNum2.ToString()))
                        {
                            color = Nowcolor;
                        }
                        else
                            if ((sta[i].num == nextNum1.ToString()) || (sta[i].num == nextNum2.ToString()))
                            color = Color.YellowGreen;
                        else
                            color = Color.Red;

                        int wordX = (int)((int.Parse(sta[i].wordx)) * scaleWidth);
                        int wordY = (int)((int.Parse(sta[i].wordy)) * scaleHeight);
                        int circleX = (int)(int.Parse(sta[i].rectx) * scaleWidth);
                        int circleY = (int)(int.Parse(sta[i].recty) * scaleHeight);
                        int zhijing = int.Parse(sta[i].size);

                        circleX -= zhijing / 2;
                        circleY -= zhijing / 2;
                        Brush bush = new SolidBrush(Color.Blue);//填充的颜色

                        //在图片上写字
                        font = new Font(fm, 32, FontStyle.Regular, GraphicsUnit.Pixel);
                        if (checkBoxbianhao.Checked)
                            gra.DrawString(sta[i].num, font, bush, new Point(wordX, wordY));

                        bush = new SolidBrush(color);//填充的颜色
                                                     //在图片上画框
                        gra.FillEllipse(bush, circleX, circleY, zhijing, zhijing);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50
                        bush = new SolidBrush(Color.Black);//填充的颜色
                        font = new Font(fm, 18, FontStyle.Regular, GraphicsUnit.Pixel);
                        if (sta[i].num == NowNum1.ToString())
                        {
                            //在图片上写字
                            gra.DrawString("1", font, bush, new Point(circleX + 2, circleY));
                        }

                        if (sta[i].num == nextNum1.ToString())
                        {
                            //在图片上写字
                            gra.DrawString("1", font, bush, new Point(circleX + 2, circleY));
                        }


                        if (sta[i].num == NowNum2.ToString())
                        {
                            //在图片上写字
                            gra.DrawString("2", font, bush, new Point(circleX + 2, circleY));
                        }

                        if (sta[i].num == nextNum2.ToString())
                        {
                            //在图片上写字
                            gra.DrawString("2", font, bush, new Point(circleX + 2, circleY));
                        }


                    }
                    //gra.Dispose();
                }
                else//没有站点信息
                {
                    pictureBox1.Image = bitmap;
                }
            }
            catch
            { }
        }

        Color Nowcolor;
        bool nowstate;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nowstate)
            {
                nowstate = false;
                Nowcolor = Color.Red;
            }
            else
            {
                nowstate = true;
                Nowcolor = Color.Yellow;
            }
            updateToPic();
        }

        Int32 NowNum1 = 0;//当前地标
        Int32 nextNum1 = 0;//下一个地标

        Int32 NowNum2 = 0;//当前地标
        Int32 nextNum2 = 0;//下一个地标
        private void timer2_Tick(object sender, EventArgs e)
        {
            //刷新当前地标
            NowNum1 = PlcData1[0];//当前地标
            nextNum1 = PlcData1[17];//下一个


            NowNum2 = PlcData2[0];//当前地标
            nextNum2 = PlcData2[17];//下一个
            textBoxTime.Text = System.DateTime.Now.ToLongDateString() + "   " + System.DateTime.Now.ToLongTimeString();
        }



        void Button6Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
        }

        //编辑地图界面
        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(2);
        }

        void Button15Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(4);
        }

        void TabControl1SelectedIndexChanged(object sender, EventArgs e)
        {
            TotalNum = userGetNum();
            pictureBox1.Image = bitmap;
            NowNum1 = 1;
            NowNum2 = 1;
        }
        #endregion

        #region 地图编辑
        //更新站点设置信息到图片
        private void updateToPic2()
        {
            try
            {
                TotalNum = userGetNum();
                //读取站点信息
                if (TotalNum > 0)
                {
                    Color color = Color.Red;
                    Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    station[] sta = new Form1.station[TotalNum];
                    sta = userGetAll();
                    for (Int16 i = 0; i < TotalNum; i++)
                    {
                        if (sta[i].id == mapNowNum.ToString())
                            color = Color.YellowGreen;
                        else
                            color = Color.Blue;

                        int wordX = int.Parse(sta[i].wordx);
                        int wordY = int.Parse(sta[i].wordy);
                        int circleX = int.Parse(sta[i].rectx);
                        int circleY = int.Parse(sta[i].recty);

                        int zhijing = int.Parse(sta[i].size);

                        circleX -= zhijing / 2;
                        circleY -= zhijing / 2;


                        //在图片上写字
                        //Arial字体，大小为32，红色。
                        FontFamily fm = new FontFamily("Arial");
                        Font font = new Font(fm, 32, FontStyle.Regular, GraphicsUnit.Pixel);
                        SolidBrush sb = new SolidBrush(color);
                        g.DrawString(sta[i].num, font, sb, new PointF(wordX, wordY));

                        //在图片上画框
                        Brush bush = new SolidBrush(color);//填充的颜色
                        g.FillEllipse(bush, circleX, circleY, zhijing, zhijing);
                    }

                    textBox_MapTotalNum.Text = TotalNum.ToString();//总站点数
                    textBox_RFID_Num.Text = sta[mapNowNum - 1].num;//当前地标号
                    textBoxNowNum.Text = mapNowNum.ToString();//当前编号

                    textBox2.Text = sta[mapNowNum - 1].size;

                    label_MapWordX.Text = sta[mapNowNum - 1].wordx;
                    label_MapWordY.Text = sta[mapNowNum - 1].wordy;
                    label_MapRectX.Text = sta[mapNowNum - 1].rectx;
                    label_MapRectY.Text = sta[mapNowNum - 1].recty;

                    pictureBox2.Image = bmp;
                    g.Dispose();
                }
                else//没有站点信息
                {
                    pictureBox2.Image = bitmap;
                }
            }
            catch
            {
            }
        }

        //加载路线地图
        private void button_LoadMape_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "(All files (*.*)|*.*"; //过滤文件类型
            fd.InitialDirectory = Application.StartupPath + "\\Temp\\";//设定初始目录
            fd.ShowReadOnly = true; //设定文件是否只读
            DialogResult r = fd.ShowDialog();
            if (r == DialogResult.OK)
            {
                try
                {
                    //存储地图到本地
                    Bitmap bp = new Bitmap(fd.FileName);
                    bp.Save(mapFileName);
                    bitmap = new Bitmap(bp);
                    bp.Dispose();

                    pictureBox2.BackgroundImage = bitmap;
                    updateToPic2();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show("更新失败！");
                }
            }
        }

        //编辑地图
//        int MapRFID_Num = 1;//地标对应的存储编号，也是当前地标对应的编号
        int mapNowNum = 1;//地图编辑模式当前地标编号
        private void button_StartAdd_Click(object sender, EventArgs e)
        {
            if (button_StartAdd.Text == "添加地标")
            {
                button_StartAdd.Text = "完  成";
                TotalNum++;
                textBox_MapTotalNum.Text = TotalNum.ToString();//地标总数加1
                textBox_RFID_Num.Text = string.Empty;//清空地标号
                mapNowNum = TotalNum;
                Nowstation = new Form1.station();

                Nowstation = userfindById((mapNowNum - 1).ToString());
                Nowstation.id = mapNowNum.ToString();
                userAdd(Nowstation);
            }
            else
            {
                button_StartAdd.Text = "添加地标";

                //存储数据(更新)
                Nowstation.id = TotalNum.ToString();
                Nowstation.num = textBox_RFID_Num.Text;
                Nowstation.size = textBox2.Text;
                Nowstation.wordx = label_MapWordX.Text;
                Nowstation.wordy = label_MapWordY.Text;
                Nowstation.rectx = label_MapRectX.Text;
                Nowstation.recty = label_MapRectY.Text;
                userUpadta(Nowstation);
                //显示
                updateToPic2();
            }
        }
        void button_Map_Change_Click(object sender, EventArgs e)
        {
            Ini.Filename = "\\Map\\MapInfo.ini";
            if (button_Map_Change.Text == "更改地标")
            {
                button_Map_Change.Text = "完  成";
            }
            else
            {
                button_Map_Change.Text = "更改地标";

                //更新数据
                Nowstation.id = textBoxNowNum.Text;
                Nowstation.num = textBox_RFID_Num.Text;
                Nowstation.size = textBox2.Text;
                Nowstation.wordx = label_MapWordX.Text;
                Nowstation.wordy = label_MapWordY.Text;
                Nowstation.rectx = label_MapRectX.Text;
                Nowstation.recty = label_MapRectY.Text;
                userUpadta(Nowstation);

                //显示
                updateToPic2();
            }
        }
        void button_MapDelete_Click(object sender, EventArgs e)
        {
            try
            {
                TotalNum = userGetNum();
                if (TotalNum == 0)
                {
                    MessageBox.Show("没有地标可以删除了！");
                }
                else
                {
                    MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                    DialogResult dr = MessageBox.Show("确定删除吗?", "删除地标", messButton);
                    if (dr == DialogResult.OK)//如果点击“确定”按钮
                    {
                        if (TotalNum == 1)
                        {
                            mapNowNum = 0;
                            textBox_MapTotalNum.Text = "0";
                        }
                        else
                        {
                            //存储信息
                            if (mapNowNum == TotalNum)
                            {
                                userDel(mapNowNum.ToString());
                                TotalNum--;
                                mapNowNum--;
                            }
                            else
                            {
                                station[] sta = new Form1.station[TotalNum];
                                sta = userGetAll();
                                for (int i = mapNowNum; i < TotalNum; i++)
                                {
                                    sta[i].id = sta[i - 1].id;
                                    userUpadta(sta[i]);
                                }
                                userDel(TotalNum.ToString());
                                TotalNum--;
                            }
                        }
                        updateToPic2();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void button_map_last_Click(object sender, EventArgs e)
        {
            if (mapNowNum == 1)
                mapNowNum = TotalNum;
            else
                mapNowNum--;
            updateToPic2();
        }
        void button_map_next_Click(object sender, EventArgs e)
        {
            mapNowNum++;
            if (mapNowNum > TotalNum)
                mapNowNum = 1;
            updateToPic2();
        }
        //编辑地图模式
        private void pictureBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (textBox_MapTotalNum.Text != "0")
            {
                Rectangle rt = new Rectangle();
                for (int i = 1; i <= TotalNum; i++)
                {
                    station sta = new Form1.station();
                    sta = userfindById(i.ToString());
                    rt.X = int.Parse(sta.rectx);
                    rt.Y = int.Parse(sta.recty);

                    rt.Width = int.Parse(sta.size);
                    rt.Height = int.Parse(sta.size);

                    if (e.X >= (rt.X - rt.Width / 2.0) && e.X <= (rt.Width / 2.0 + rt.X) && e.Y >= (rt.Y - rt.Height / 2.0) && e.Y <= (rt.Height / 2.0 + rt.Y))
                    {
                        mapNowNum = i;
                        updateToPic2();
                    }
                }
            }
        }
        int withx;
        int heithty;
        int withx2;
        int heithty2;
        bool movestate = false;
        station Nowstation;
        void PictureBox2MouseDown(object sender, MouseEventArgs e)
        {
            if (button_StartAdd.Text == "完  成" || button_Map_Change.Text == "完  成")
            {
                if (textBox_RFID_Num.Text == string.Empty)
                    MessageBox.Show("提示：请先输入地标编号！");
                else
                {
                    if (radioButton_setWord.Checked)
                    {
                        label_MapWordX.Text = e.X.ToString();
                        label_MapWordY.Text = e.Y.ToString();
                    }
                    else
                    {
                        label_MapRectX.Text = e.X.ToString();
                        label_MapRectY.Text = e.Y.ToString();
                    }
                    if (movestate == false)
                    {
                        movestate = true;
                        pictureBox2.BackgroundImage = bitmap;
                    }
                }
            }
        }

        void PictureBox2MouseMove(object sender, MouseEventArgs e)
        {
            withx = e.X;
            heithty = e.Y;
            if (movestate)
            {
                try
                {
                    TotalNum = userGetNum();
                    //读取站点信息
                    if (TotalNum > 0)
                    {
                        Color color = Color.Red;

                        Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                        Graphics g = Graphics.FromImage(bmp);
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        Brush bush = new SolidBrush(Color.Blue);//填充的颜色

                        station[] sta = new Form1.station[TotalNum];
                        sta = userGetAll();
                        for (Int16 i = 0; i < TotalNum; i++)
                        {
                            if (sta[i].id == mapNowNum.ToString())
                            {
                                color = Color.YellowGreen;
                                sta[i].num = textBox_RFID_Num.Text;
                            }
                            else
                                color = Color.Blue;

                            int zhijing = 0;
                            int wordX = 0;
                            int wordY = 0;
                            int circleX = 0;
                            int circleY = 0;

                            if (sta[i].id == mapNowNum.ToString())
                            {
                                if (radioButton_SetRect.Checked)
                                {
                                    wordX = int.Parse(label_MapWordX.Text);
                                    wordY = int.Parse(label_MapWordY.Text);
                                    circleX = withx;
                                    circleY = heithty;
                                }
                                else
                                {
                                    wordX = withx;
                                    wordY = heithty;
                                    circleX = int.Parse(label_MapRectX.Text);
                                    circleY = int.Parse(label_MapRectY.Text);
                                }

                                zhijing = int.Parse(textBox2.Text);

                                label_MapWordX.Text = wordX.ToString();
                                label_MapWordY.Text = wordY.ToString();
                                label_MapRectX.Text = circleX.ToString();
                                label_MapRectY.Text = circleY.ToString();

                            }
                            else
                            {
                                wordX = int.Parse(sta[i].wordx);
                                wordY = int.Parse(sta[i].wordy);
                                circleX = int.Parse(sta[i].rectx);
                                circleY = int.Parse(sta[i].recty);

                                zhijing = int.Parse(sta[i].size);
                            }

                            circleX -= zhijing / 2;
                            circleY -= zhijing / 2;

                            //在图片上写字
                            //Arial字体，大小为32，红色。
                            FontFamily fm = new FontFamily("Arial");
                            Font font = new Font(fm, 32, FontStyle.Regular, GraphicsUnit.Pixel);
                            SolidBrush sb = new SolidBrush(color);

                            if (radioButton_setWord.Checked && (sta[i].id == mapNowNum.ToString()))
                                g.DrawString(sta[i].num, font, sb, new PointF(wordX, wordY));
                            else
                                g.DrawString(sta[i].num, font, sb, new PointF(wordX, wordY));

                            bush = new SolidBrush(color);//填充的颜色
                                                         //在图片上画框
                            g.FillEllipse(bush, circleX, circleY, zhijing, zhijing);
                        }
                        pictureBox2.Image = bmp;
                        g.Dispose();
                    }
                    else//没有站点信息
                    {
                        pictureBox2.Image = bitmap;
                    }
                    withx2 = withx;
                    heithty2 = heithty;
                }
                catch
                { }
            }
        }

        void PictureBox2MouseUp(object sender, MouseEventArgs e)
        {
            movestate = false;
            if (button_StartAdd.Text == "完  成" || button_Map_Change.Text == "完  成")
            {
                if (textBox_RFID_Num.Text == string.Empty)
                    MessageBox.Show("提示：请先输入地标编号！");
                else
                {
                    withx = e.X;
                    heithty = e.Y;
                    if (radioButton_setWord.Checked)
                    {
                        label_MapWordX.Text = withx.ToString();
                        label_MapWordY.Text = heithty.ToString();
                    }
                    else
                    {
                        label_MapRectX.Text = withx.ToString();
                        label_MapRectY.Text = heithty.ToString();
                    }
                }
            }
        }
        #endregion

        void CheckBoxbianhaoCheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxbianhao.Checked == false)
            {
                pictureBox1.Refresh();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th != null)
            {
                th.Abort();
            }
        }

        #region 电梯操作按键
        //一楼外呼
        void ButtonylhjClick(object sender, EventArgs e)
        {
            PlcData3[16] = 0;
        }
        //三楼外呼
        void ButtonslhjClick(object sender, EventArgs e)
        {
            PlcData3[15] = 0;
        }
        //1按钮
        void ButtonqylClick(object sender, EventArgs e)
        {
            PlcData3[19] = 0;
        }
        //3按钮
        void ButtonyslClick(object sender, EventArgs e)
        {
            PlcData3[17] = 0;
        }
        //关门
        void ButtongmClick(object sender, EventArgs e)
        {
            PlcData3[13] = 0;
        }
        //开门
        void ButtonkmClick(object sender, EventArgs e)
        {
            PlcData3[14] = 0;
        }
        #endregion 

        void Button3Click(object sender, EventArgs e)
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要退出吗?", "退出系统", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                Application.Exit();
            }
        }

        void RadioButtonDUCheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDU.Checked)
            {
                buttonylhj.Enabled = true;
                buttonslhj.Enabled = true;
                buttonqyl.Enabled = true;
                buttonqsl.Enabled = true;
                buttonkm.Enabled = true;
                buttongm.Enabled = true;
            }
            else
            {
                buttonylhj.Enabled = false;
                buttonslhj.Enabled = false;
                buttonqyl.Enabled = false;
                buttonqsl.Enabled = false;
                buttonkm.Enabled = false;
                buttongm.Enabled = false;
            }
        }
		
        //定时1分钟，连接失败1分钟后自动重连
        int timecount=0;
		void Timer3Tick(object sender, EventArgs e)
		{
			timecount++;
			if(timecount==50)
			{
				timecount = 0;
				robot1.Checked = true;
				robot2.Checked = true;
				robot3.Checked =true;
				robot4.Checked =true;
				
				agvSensor1.Checked=true;
				agvSensor2.Checked=true;
				
				checkBox3.Checked=true;
			}
		}
    }
}