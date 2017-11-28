using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modustest
{
    public partial class AGVSensor : UserControl
    {
        /// <summary>
        /// AGV序号
        /// </summary>
        private int agvNumber;
        public int AgvNumber
        {
        	get 
        	{
        		return agvNumber;
        	}
        	set
        	{
        		if(agvNumber!=value)
        		{
        			agvNumber = value;
        			labelAgvNum.Text = agvNumber.ToString();
        		}
        	}
        }
        
        /// <summary>
        /// AGV序号
        /// </summary>
        private int agvID;
        public int AgvID
        {
        	get 
        	{
        		return agvID;
        	}
        	set
        	{
        		if(agvID!=value)
        		{
        			agvID = value;
        			labelID.Text = agvID.ToString();
        		}
        	}
        }
        
        /// <summary>
        /// 在线
        /// </summary>
        private bool agvOnline;
        public bool AgvOnline
        {
        	get 
        	{
        		return agvOnline;
        	}
        	set
        	{
        		if(agvOnline!=value)
        		{
        			agvOnline = value;
        			if(value)
        			{
		                button_online.Text = "在线";
		                button_online.BackColor = Color.YellowGreen;
		                //车身颜色
			        	button23.BackColor = Color.OrangeRed;
			        	
			        	//红外
			        	label29.BackColor =Color.OrangeRed;
			        	label30.BackColor=Color.OrangeRed;
			        	
			        	 // 左叉臂避障
				        if (zuoChaBiBiZhang!=0) 
				        	button_zcbbz.BackColor =  Color.Green;
				        else
				        	button_zcbbz.BackColor =  Color.Red;
				        
				        // 右叉臂避障
				        if (youChaBiBiZhang!=0) 
				        	button_ycbbz.BackColor =  Color.Green;
				        else
				        	button_ycbbz.BackColor =  Color.Red;
				        
				        // 左叉臂检测
				        if (zuoChaBiJianCe!=0) 
				        	button_zcbjc.BackColor =  Color.Green;
				        else
				        	button_zcbjc.BackColor =  Color.Red;
				        
				        // 右叉臂检测
				        if (youChaBiJianCe!=0) 
				        	button_ycbjc.BackColor =  Color.Green;
				        else
				        	button_ycbjc.BackColor =  Color.Red;
				        
				        
				        // 远程避障
				        if (yuanChengBiZhang!=0) 
				        	button_ybz.BackColor =  Color.Green;
				        else
				        	button_ybz.BackColor =  Color.Red;
				        
				        
				        // 近程避障
				        if (jinChengBiZhang!=0) 
				        	button_jbz.BackColor =  Color.Green;
				        else
				        	button_jbz.BackColor =  Color.Red;
				        
				        
				        // 机械避障
				        if (jiXieBiZhang==0) 
				        	button_jxbz.BackColor =  Color.Green;
				        else
				        	button_jxbz.BackColor =  Color.Red;    
        			}
        			else
        			{
		                button_online.Text = "离线";
		                button_online.BackColor = Color.LightCoral;
		                //车身颜色
			        	button23.BackColor = Color.LightCoral;
			        	
			        	//红外
			        	label29.BackColor =Color.LightCoral;
			        	label30.BackColor=Color.LightCoral;
			        	
			        	
			        	button_zcbbz.BackColor =  Color.LightGray;
			        	button_ycbbz.BackColor= Color.LightGray;
			        	button_zcbjc.BackColor=Color.LightGray;
			        	button_ycbjc.BackColor=Color.LightGray;
			        	
			        	button_ybz.BackColor=Color.LightGray;
			        	button_jbz.BackColor=Color.LightGray;
			        	
			        	button_jxbz.BackColor=Color.LightGray;
        			}
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
        /// 左叉臂避障
        /// </summary>
        private int zuoChaBiBiZhang;
        public int ZuoChaBiBiZhang
        {
        	get 
        	{
        		return zuoChaBiBiZhang;
        	}
        	set
        	{
        		if(zuoChaBiBiZhang!=value)
        		{
        			zuoChaBiBiZhang = value;
        			 // 左叉臂避障
			        if (zuoChaBiBiZhang!=0) 
			        	button_zcbbz.BackColor =  Color.Green;
			        else
			        	button_zcbbz.BackColor =  Color.Red;
        		}
        	}
        }

        /// <summary>
        /// 右叉臂避障
        /// </summary>
        private int youChaBiBiZhang;
        public int YouChaBiBiZhang
        {
        	get 
        	{
        		return youChaBiBiZhang;
        	}
        	set
        	{
        		if(youChaBiBiZhang!=value)
        		{
        			youChaBiBiZhang = value;
        			 // 右叉臂避障
			        if (youChaBiBiZhang!=0) 
			        	button_ycbbz.BackColor =  Color.Green;
			        else
			        	button_ycbbz.BackColor =  Color.Red;
        		}
        	}
        }
        
        /// <summary>
        /// 左叉臂检测
        /// </summary>
        private int zuoChaBiJianCe;
		public int ZuoChaBiJianCe
		{
			get 
        	{
        		return zuoChaBiJianCe;
        	}
        	set
        	{
        		if(zuoChaBiJianCe!=value)
        		{
        			zuoChaBiJianCe = value;
        			// 左叉臂检测
			        if (zuoChaBiJianCe!=0) 
			        	button_zcbjc.BackColor =  Color.Green;
			        else
			        	button_zcbjc.BackColor =  Color.Red;
        		}
        	}
		}
        /// <summary>
        /// 右叉臂检测
        /// </summary>
		private int youChaBiJianCe;
		public int YouChaBiJianCe
		{
			get 
        	{
        		return youChaBiJianCe;
        	}
        	set
        	{
        		if(youChaBiJianCe!=value)
        		{
        			youChaBiJianCe = value;
        			// 右叉臂检测
			        if (youChaBiJianCe!=0) 
			        	button_ycbjc.BackColor =  Color.Green;
			        else
			        	button_ycbjc.BackColor =  Color.Red;
        		}
        	}
		}
		
        /// <summary>
        /// 远程避障
        /// </summary>
      	private int yuanChengBiZhang;
      	public int YuanChengBiZhang
      	{
      		get 
        	{
        		return yuanChengBiZhang;
        	}
        	set
        	{
        		if(yuanChengBiZhang!=value)
        		{
        			yuanChengBiZhang = value;
        			// 远程避障
			        if (yuanChengBiZhang!=0) 
			        	button_ybz.BackColor =  Color.Green;
			        else
			        	button_ybz.BackColor =  Color.Red;
        		}
        	}
      	}
      	
        /// <summary>
        /// 近程避障
        /// </summary>
        private int jinChengBiZhang;
		public int JinChengBiZhang
		{
			get 
        	{
        		return jinChengBiZhang;
        	}
        	set
        	{
        		if(jinChengBiZhang!=value)
        		{
        			jinChengBiZhang = value;
        			// 近程避障
			        if (jinChengBiZhang!=0) 
			        	button_jbz.BackColor =  Color.Green;
			        else
			        	button_jbz.BackColor =  Color.Red;
        		}
        	}
		}
        
        
        /// <summary>
        /// 机械避障
        /// </summary>
        private int jiXieBiZhang;
        public int JiXieBiZhang
		{
			get 
        	{
        		return jiXieBiZhang;
        	}
        	set
        	{
        		if(jiXieBiZhang!=value)
        		{
        			jiXieBiZhang = value;
        			// 机械避障
			        if (jiXieBiZhang==0) 
			        	button_jxbz.BackColor =  Color.Green;
			        else
			        	button_jxbz.BackColor =  Color.Red;    
        		}
        	}
		}
                
        public AGVSensor()
        {
            InitializeComponent();
        }
       
        
        void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
        	//如果是不连接，显示离线
			if(checkBox1.Checked==false)
			{
				button_online.Text = "离线";
                button_online.BackColor = Color.LightCoral;
                //车身颜色
	        	button23.BackColor = Color.LightCoral;
	        	
	        	//红外
	        	label29.BackColor =Color.LightCoral;
	        	label30.BackColor=Color.LightCoral;
	        	
	        	
	        	button_zcbbz.BackColor =  Color.LightGray;
	        	button_ycbbz.BackColor= Color.LightGray;
	        	button_zcbjc.BackColor=Color.LightGray;
	        	button_ycbjc.BackColor=Color.LightGray;
	        	
	        	button_ybz.BackColor=Color.LightGray;
	        	button_jbz.BackColor=Color.LightGray;
	        	
	        	button_jxbz.BackColor=Color.LightGray;
			}
        }
    }
}
