/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2017/6/25
 * Time: 14:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Modustest
{
	/// <summary>
	/// Description of Robot.
	/// </summary>
	public partial class Robot : UserControl
	{		
        /// <summary>
        /// 设备名称
        /// </summary>
      	private string _DeviceName;
        public String DeviceName
        {
        	get 
        	{
        		return _DeviceName;
        	}
        	set
        	{
        		if(_DeviceName!=value)
        		{
        			_DeviceName = value;
        			groupBox1.Text = _DeviceName;
        		}
        	}
        }

        /// <summary>
        /// 在线
        /// </summary>
        private bool _DeviceOnline;
        public bool DeviceOnline
        {
        	get
        	{
        		return _DeviceOnline;
        	}
        	set
        	{
        		
        			_DeviceOnline = value;
        			
        			if (checkBox1.Checked)
        			{
        			
        				if(_DeviceOnline)
        			
        				{
	        				button2zx.Text = "在线";
		                	button2zx.BackColor = Color.YellowGreen;
		                	
		                	//请求进入
					        if (_QingQiuJinRu!=0) 
					         	labelqqjr.ForeColor =  Color.YellowGreen;
					        else
					            labelqqjr.ForeColor =  Color.Red;
					       
					        //进入工作区
					        if (_JinRuGongZuoQu!=0) 
					        	labeljrgzq.ForeColor =  Color.YellowGreen;
					        else
					            labeljrgzq.ForeColor =  Color.Red;
					       
					        //允许进入
					        if (_YunXuJinRu!=0) 
					        	labelyxjr.ForeColor =  Color.YellowGreen;
					        else
					            labelyxjr.ForeColor =  Color.Red;
					        
					        //启动
					        if (_QiDong!=0) 
					        	label_qidong.ForeColor =  Color.YellowGreen;
					        else
					            label_qidong.ForeColor =  Color.Red;
        				}
	        			else
	        			{
	        				button2zx.Text = "离线";
		                	button2zx.BackColor = Color.LightCoral;
		                	
			                //请求进入
					        labelqqjr.ForeColor =  Color.LightGray;
					       
					        //进入工作区
					        labeljrgzq.ForeColor =  Color.LightGray;
					       
					        //允许进入
					        labelyxjr.ForeColor =  Color.LightGray;
					        
					        //启动
		        			label_qidong.ForeColor =  Color.LightGray;
	        			}        				
        			}

        		}
        	
        }

        /// <summary>
        /// 请求进入
        /// </summary>
        private int _QingQiuJinRu;
        public int QingQiuJinRu
        {
        	get 
        	{
        		return _QingQiuJinRu;
        	}
        	set
        	{
        		if(_QingQiuJinRu!=value)
        		{
        			_QingQiuJinRu = value;
			        if (_QingQiuJinRu!=0) 
			         	labelqqjr.ForeColor =  Color.YellowGreen;
			        else
			            labelqqjr.ForeColor =  Color.Red;
        		}
        	}
        }
        /// <summary>
        /// 进入工作区
        /// </summary>
        private int _JinRuGongZuoQu;
        public int JinRuGongZuoQu
        {
        	get 
        	{
        		return _JinRuGongZuoQu;
        	}
        	set
        	{
        		if(_JinRuGongZuoQu!=value)
        		{
        			_JinRuGongZuoQu = value;
			        if (_JinRuGongZuoQu!=0) 
			         	labeljrgzq.ForeColor =  Color.YellowGreen;
			        else
			            labeljrgzq.ForeColor =  Color.Red;
        		}
        	}
        }
        /// <summary>
        /// 允许进入
        /// </summary>
        private int _YunXuJinRu;
        public int YunXuJinRu
        {
        	get 
        	{
        		return _YunXuJinRu;
        	}
        	set
        	{
        		if(_YunXuJinRu!=value)
        		{
        			_YunXuJinRu = value;
			        if (_YunXuJinRu!=0) 
			         	labelyxjr.ForeColor =  Color.YellowGreen;
			        else
			            labelyxjr.ForeColor =  Color.Red;
        		}
        	}
        }
        
        /// <summary>
        /// 启动
        /// </summary>
        private int _QiDong;
        public int QiDong
        {
        	get 
        	{
        		return _QiDong;
        	}
        	set
        	{
        		if(_QiDong!=value)
        		{
        			_QiDong = value;
			        if (_QiDong!=0) 
			         	label_qidong.ForeColor =  Color.YellowGreen;
			        else
			            label_qidong.ForeColor =  Color.Red;
        		}
        	}
        }
        /// <summary>
        /// 连接设备
        /// </summary>
        public bool Checked
        {
        	get 
        	{
        		return checkBox1.Checked;
        	}
        	set
        	{
        		if(checkBox1.Checked!=value)
        		{
        			checkBox1.Checked = value;
        		}
        	}
        }
        /// <summary>
        /// 设备ID
        /// </summary>
      	private int _DeviceID;
        public int DeviceID
        {
        	get 
        	{
        		return _DeviceID;
        	}
        	set
        	{
        		if(_DeviceID!=value)
        		{
        			_DeviceID = value;
        			labelDeviceID.Text = _DeviceID.ToString();
        		}
        	}
        }
        
		public Robot()
		{
			InitializeComponent();

		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			//如果是不连接，显示离线
			if(checkBox1.Checked==false)
			{
				button2zx.Text = "离线";
	        	button2zx.BackColor = Color.LightGray;
	        	
	            //请求进入
		        labelqqjr.ForeColor =  Color.LightGray;
		       
		        //进入工作区
		        labeljrgzq.ForeColor =  Color.LightGray;
		       
		        //允许进入
		        labelyxjr.ForeColor =  Color.LightGray;
		        
		        //启动
		        label_qidong.ForeColor =  Color.LightGray;
			}
		}
	}
}
