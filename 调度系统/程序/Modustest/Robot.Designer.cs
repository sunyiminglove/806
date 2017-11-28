/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2017/6/25
 * Time: 14:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Modustest
{
	partial class Robot
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labeljrgzq = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.labelDeviceID = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.labelyxjr = new System.Windows.Forms.Label();
			this.labelqqjr = new System.Windows.Forms.Label();
			this.button2zx = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label_qidong = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.label_qidong);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.labeljrgzq);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Controls.Add(this.labelDeviceID);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.labelyxjr);
			this.groupBox1.Controls.Add(this.labelqqjr);
			this.groupBox1.Controls.Add(this.button2zx);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Font = new System.Drawing.Font("宋体", 12F);
			this.groupBox1.Location = new System.Drawing.Point(3, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(240, 117);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "设备";
			// 
			// labeljrgzq
			// 
			this.labeljrgzq.AutoSize = true;
			this.labeljrgzq.Font = new System.Drawing.Font("宋体", 14F);
			this.labeljrgzq.ForeColor = System.Drawing.Color.LightGray;
			this.labeljrgzq.Location = new System.Drawing.Point(212, 31);
			this.labeljrgzq.Name = "labeljrgzq";
			this.labeljrgzq.Size = new System.Drawing.Size(28, 19);
			this.labeljrgzq.TabIndex = 169;
			this.labeljrgzq.Text = "●";
			// 
			// checkBox1
			// 
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Font = new System.Drawing.Font("宋体", 12F);
			this.checkBox1.Location = new System.Drawing.Point(81, 89);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(65, 24);
			this.checkBox1.TabIndex = 174;
			this.checkBox1.Text = "连接";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1CheckedChanged);
			// 
			// labelDeviceID
			// 
			this.labelDeviceID.Font = new System.Drawing.Font("宋体", 12F);
			this.labelDeviceID.Location = new System.Drawing.Point(53, 90);
			this.labelDeviceID.Name = "labelDeviceID";
			this.labelDeviceID.Size = new System.Drawing.Size(28, 24);
			this.labelDeviceID.TabIndex = 173;
			this.labelDeviceID.Text = "1";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label4.Location = new System.Drawing.Point(125, 33);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(92, 24);
			this.label4.TabIndex = 171;
			this.label4.Text = "进入工作区：";
			// 
			// labelyxjr
			// 
			this.labelyxjr.AutoSize = true;
			this.labelyxjr.Font = new System.Drawing.Font("宋体", 14F);
			this.labelyxjr.ForeColor = System.Drawing.Color.LightGray;
			this.labelyxjr.Location = new System.Drawing.Point(91, 63);
			this.labelyxjr.Name = "labelyxjr";
			this.labelyxjr.Size = new System.Drawing.Size(28, 19);
			this.labelyxjr.TabIndex = 168;
			this.labelyxjr.Text = "●";
			// 
			// labelqqjr
			// 
			this.labelqqjr.AutoSize = true;
			this.labelqqjr.Font = new System.Drawing.Font("宋体", 14F);
			this.labelqqjr.ForeColor = System.Drawing.Color.LightGray;
			this.labelqqjr.Location = new System.Drawing.Point(91, 32);
			this.labelqqjr.Name = "labelqqjr";
			this.labelqqjr.Size = new System.Drawing.Size(28, 19);
			this.labelqqjr.TabIndex = 167;
			this.labelqqjr.Text = "●";
			// 
			// button2zx
			// 
			this.button2zx.BackColor = System.Drawing.Color.LightGray;
			this.button2zx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2zx.Font = new System.Drawing.Font("宋体", 9F);
			this.button2zx.Location = new System.Drawing.Point(174, 89);
			this.button2zx.Name = "button2zx";
			this.button2zx.Size = new System.Drawing.Size(51, 22);
			this.button2zx.TabIndex = 166;
			this.button2zx.Text = "离线";
			this.button2zx.UseVisualStyleBackColor = false;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("宋体", 10.5F);
			this.label3.Location = new System.Drawing.Point(18, 33);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(82, 24);
			this.label3.TabIndex = 170;
			this.label3.Text = "请求进入：";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label5.Location = new System.Drawing.Point(17, 63);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(77, 24);
			this.label5.TabIndex = 172;
			this.label5.Text = "允许进入：";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("宋体", 12F);
			this.label7.Location = new System.Drawing.Point(18, 90);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(50, 24);
			this.label7.TabIndex = 175;
			this.label7.Text = "ID：";
			// 
			// label_qidong
			// 
			this.label_qidong.AutoSize = true;
			this.label_qidong.Font = new System.Drawing.Font("宋体", 14F);
			this.label_qidong.ForeColor = System.Drawing.Color.LightGray;
			this.label_qidong.Location = new System.Drawing.Point(212, 63);
			this.label_qidong.Name = "label_qidong";
			this.label_qidong.Size = new System.Drawing.Size(28, 19);
			this.label_qidong.TabIndex = 176;
			this.label_qidong.Text = "●";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label2.Location = new System.Drawing.Point(138, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 24);
			this.label2.TabIndex = 177;
			this.label2.Text = "启    动：";
			// 
			// Robot
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.groupBox1);
			this.Name = "Robot";
			this.Size = new System.Drawing.Size(249, 119);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label_qidong;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label labelyxjr;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelDeviceID;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label labeljrgzq;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label labelqqjr;
		private System.Windows.Forms.Button button2zx;
	}
}
