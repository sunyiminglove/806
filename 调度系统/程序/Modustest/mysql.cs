/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/11/9 星期三
 * 时间: 下午 2:25
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Modustest
{
	/// <summary>
	/// Description of mysql.
	/// </summary>
	public class mysql
	{
        /// <summary>
        /// 建立mysql数据库链接
        /// </summary>
        /// <returns></returns>
        private static MySqlConnection MySqlCon(string adress,string id,string password)
        {
            String mysqlStr = "Database=agv;Data Source="+adress+";User Id="+id+";Password="+password+";pooling=false;CharSet=utf8;port=3306";
            // String mySqlCon = ConfigurationManager.ConnectionStrings["MySqlCon"].ConnectionString;
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
        /// <summary>
        /// 查询并获得结果集并遍历
        /// </summary>
        /// <param name="mySqlCommand"></param>
        private static void getResultset(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            try
            {
                string rs = string.Empty;
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        rs += reader.GetInt32(0) + "  ";
                        for (int i = 1; i < reader.FieldCount; i++)
                            rs += reader.GetString(i) + "  ";
                        rs += "\r\n";
                    }
                }
                MessageBox.Show(rs);
            }
            catch (Exception)
            {

                Console.WriteLine("查询失败了！");
            }
            finally
            {
                reader.Close();
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="mySqlCommand"></param>
        private static void getInsert(MySqlCommand mySqlCommand)
        {
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                MessageBox.Show(message);
                Console.WriteLine("插入数据失败了！" + message);
            }

        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="mySqlCommand"></param>
        private static void getUpdate(MySqlCommand mySqlCommand)
        {
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                String message = ex.Message;
                Console.WriteLine("修改数据失败了！" + message);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="mySqlCommand"></param>
        private static void getDel(MySqlCommand mySqlCommand)
        {
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                Console.WriteLine("删除数据失败了！" + message);
            }
        }

      	public struct station
        {
        	public string num;
        	public string size;
        	public string wordx;
        	public string wordy;
        	public string rectx;
        	public string recty;
        };
        
        //用户添加数据
//        private static void userAdd(station sta)
//        {
//            try
//            {
//                //创建数据库连接对象
//                MySqlConnection mysql = MySqlCon("localhost","root","root");
//
//                //打开数据库
//                mysql.Open();
//
//                //查询用户的设备id是否存在
//                String sqlSearch = "select * from usersensor where userid='" + userid + "' and sensorid = '" + sensorid + "' limit 0,1";
//                MySqlCommand mySqlCommand = getSqlCommand(sqlSearch, mysql);
//
//                MySqlDataReader reader = mySqlCommand.ExecuteReader();
//
//                string[] update = new string[15];
//
//                if (reader.Read())//存在，更新数据，不存在不处理
//                {
//                    if (reader.HasRows)
//                    {
//                        for (int i = 6; i < 20; i++)
//                            update[i - 6] = reader.GetString(i);
//                    }
//                    reader.Close();
//                    //将最新的数据添加到队列尾部
//                    update[14] = data;
//
//                    //修改sql
//                    String sqlUpdate = "update usersensor set data1='" + update[0] + "',data2='" + update[1] + "',data3='" + update[2] + "', data4='" + update[3] + "' ,data5='" + update[4] + "', data6='" + update[5] + "', data7='" + update[6] + "', data8='" + update[7] + "', data9='" + update[8] + "', data10='" + update[9] + "', data11='" + update[10] + "' ,data12='" + update[11] + "', data13='" + update[12] + "',data14='" + update[13] + "',data15='" + update[14] + "'where userid='" + userid + "' and sensorid = '" + sensorid + "'";
//                    mySqlCommand = getSqlCommand(sqlUpdate, mysql);
//                    mySqlCommand.ExecuteNonQuery();
//                }
//                //关闭
//                mysql.Close();
//            }
//            catch
//            { }
//        }
        
         //用户删除数据
         
         
         /// <summary>
         /// 用户查询数据
         /// </summary>
         /// <param name="num">地标号</param>
         public station userfind(string num)
        {
         	station sta=new mysql.station();
            try
            {
                //创建数据库连接对象
                MySqlConnection mysql = MySqlCon("localhost","root","root");

                //打开数据库
                mysql.Open();

                //查询数据
                String sqlSearch = "select * from station where num='" + num + "' limit 0,1";
                MySqlCommand mySqlCommand = getSqlCommand(sqlSearch, mysql);

                MySqlDataReader reader = mySqlCommand.ExecuteReader();

                string[] update = new string[15];

                
                sta.num = reader.GetSByte("num").ToString();
                sta.size = reader.GetSByte("size").ToString();
                sta.wordx = reader.GetSByte("wordx").ToString();
                sta.wordy = reader.GetSByte("wordy").ToString();
                sta.rectx = reader.GetSByte("rectx").ToString();
                sta.recty = reader.GetSByte("recty").ToString();
                
                //关闭
                mysql.Close();
            }
            catch
            { }
            return sta;
        }
	}
}
