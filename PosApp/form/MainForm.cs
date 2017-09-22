﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using LotPos.back;

namespace LotPos
{
    public partial class MainForm : Form
    {
        /* 定义类 
         */
        //public LogOnForm logonform;     //登录界面类
        //后台业务处理类
        PosConfig posconfig;        //配置文件类 
        SocketClass betsock = new SocketClass();

        /* 控件参数类
         */
        TextBox nownumbox = null;       //当期输入框控件

        int betcount;

        int heartbeatdt = 0;
        
        string sendbetstr = "";
        static int sequence = 0;
        string sk = "111111";
        byte[] btmsg = new byte[1024];
        string smsg2 = "";
        string ServerIP = "10.1.1.192";
        //int selport = 9902;
        int betport = 9901;
        static int _lsh = 1;

        public MainForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 加载主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            AtLogonForm(1);//加载登录界面  
            
            tabControl1.Visible = true;     //标签控制页


            ShowPage_1(1);
            Update_panel_Parameters_Show(1); //
             
            
        }

        /// <summary>
        /// 处理登录界面返回结果
        /// </summary>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        void AtLogonForm(int pageNum)
        {
        }

        
         

        /// <summary>
        /// 站点玩法等参数显示
        /// </summary>
        void Update_panel_Parameters_Show(int wf)
        {
            GameName.Text = "玩法";
            DrawNo.Text = "期号";
            AgentId.Text = "站号";
            Lsh.Text = "流水号";
            SmallCount.Text = @"/" + betcount;     //小计
            Balance.Text = "余额";
            TQTime.Text = "特权时间";
            
            panelC515_Parameters.Visible = true;    //显示参数区域

        }

        /// <summary>
        /// 右上角信息栏时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Dtime_tick(object sender, EventArgs e)
        {

        }


        void ShowPage_1(int pagenum)
        {
            tabControl1.SelectTab(1);
            tabControl1.SelectedTab.Controls.Add(panelC515_Parameters);
            panelC515_Parameters.Show();
            panel_Bet.Show();
            panel_keyboard.Show();
        }

        void PanelParameters()
        {

        }

              
        /// <summary>
        /// 登录\注销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Logonoff_Click(object sender, EventArgs e)
        { 
        }

        /// <summary>
        /// 投注确认 显示票面
        /// </summary>
        void Betqueren()
        {
            if (true)
            {
                groupBox_LotteryPicture.BackColor = Color.FromArgb(255, 192, 192); PaintPicture();
                 Label lab = new Label();
                //groupBox_LotteryPicture.Controls.Add(lab);
                //BetNum betnumclass = new BetNum();
                //for (int i = 0; i < betnumclass.lstbetnum.Count; i++)
                //{
                //    lab.Text = betnumclass.lstbetnum[i];
                //}
            }
        }


        //select 
        private void Button1_Click(object sender, EventArgs e)
        {
            Issuequery_msg();
        }

        //showbet button 生成投注号码 、 检查投注号码 、 生成发送串 
        private void Btn_test_Click(object sender, EventArgs e)
        { 
        }

        //新期查询 暂未启用 lias投注版本的
        private void Issuequery_msg()
        {
            //SocketClass selsock = new SocketClass();

            //string shead = "1|1|0|003170|ISSUEQUERY|" + agentidbox.Text + "|";
            //string sbody = agentidbox.Text + "$" + gamenamebox.Text + "$" + "2017020$"; 
            //string s1 = shead + sbody + sk;
            //StringBuilder s2 = new StringBuilder(512);

            ////BzWebSec bzsec = new BzWebSec();//不需要new 因为bzwebsec中这些方法是static
            //BzWebSec.WebMD5String32(s1, s2);
            //string szy = s2.ToString();
            //int stp = -1;
            //stp = BzWebSec.WebEncodeString(sbody, sk, s2);
            //string msbody = s2.ToString();
            //int imsglen = shead.Length + szy.Length + msbody.Length + 1;
            //string msglen = "@" + (imsglen.ToString()).PadLeft(4, '0') + "|";
            //smsg2 = msglen + shead + msbody + "|" + szy;
            ////建socket链接
            //if ( 0 == selsock.inisocket(ServerIP, selport))
            //{
            //    //发送send
            //    selsock.sendmsg(smsg2);
            //    textBox3.Text += "send message is:" + smsg2 + "\r\n";
            //    //接受recv
            //    string srecmsg = selsock.recvmsg();
            //    textBox4.Text += "\r\nreceive message is:" + srecmsg + "\r\n";
            //    string[] sArray = srecmsg.Split('|');
            //    string srembody = sArray[7];
            //    BzWebSec.WebDecodeString(srembody, sk, s2);
            //    string srebody = s2.ToString();
            //    textBox4.Text += "\r\n" + srebody;
            //    selsock.closesock();
            //}
            //else
            //{
            //    textBox4.Text += " " + "\r\n" + SocketClass.errstring;
            //}
        }

        #region 检查选号
        private int Check_ball(string sredball, string sblueball, ref string str1, ref string str2)
        {
            string sred = sredball;
            string sblu = sblueball;
            int rballen = sred.Length;
            int blulen = sblu.Length;
            if (rballen % 2 != 0)
            {
                return -1;
            }
            if (blulen % 2 != 0)
            {
                return -2;
            }
            string sball = sred + sblu;
            int[] ckball = new int[rballen + blulen];
            for (int i = 0; i < rballen; i++)
            {
                ckball[i] = Convert.ToInt16(sball.Substring(i, 2));
                if (i > 0)
                {
                    if (ckball[i] == ckball[i - 1])
                    {
                        return -3;//投注号码有重复
                    }
                }
            }

            str1 = "";
            str2 = "";
            return 0;
        }
        #endregion

        #region 初始化投注串
        private string Ini_betstr(string sball)
        {

            //330106$$$33010620170525145853000001$1$0$$ $ $ $ $01$ $ 
            #region 注释部分,串示例说明
            /*
             *  agentid         330106
             *  gamename        ql515
             *  drawno          2017020
             *  ticket          33010620170525145853000001
             *  playtype 1
             *  money 2
             *  betdetail 
             *  name
             *  phonenumber
             *  idnumber
             *  cardnumber
             *  reserv1
             *  reserv2
             *  reserv3
             */
            #endregion
            DateTime.Now.ToShortTimeString();
            DateTime dt = DateTime.Now;

            string agentid = AgentId.Text;           //渠道编号
            string gamename = GameName.Text;         //玩法编号
            string drawno = DrawNo.Text;             //期号
            string ticket = AgentId.Text + string.Format("{0:yyyyMMddHHmmss}", dt) + (_lsh.ToString()).PadLeft(6, '0');         //票ID

            BetNum betnum = new BetNum();
            string playtype = "";                       //投注方式
            int money = 0;                              //金额
            //int ret = betnum.cfof_check_ball(sball, ref playtype, ref money);       
            //检查投注号码合法 + 计算投注方式和金额        
            if (0 == betnum.Cfof_check_ball(sball, ref playtype, ref money))
            {
                string betdetail =  (sball.Length / 2).ToString().PadLeft(2, '0') + sball;            //号码串   （倍数+号码个数+号码）
                money = money ;
                string smoney = string.Format("{0:f2}", money);
                string betmsgbody = agentid + "$" + gamename + "$" + drawno + "$" + ticket + "$" + playtype + "$" + smoney + "$" + betdetail + "$" + "$" + "$" + "$" + "$" + "01" + "$" + "$";
                textBox3.Text += "\r\nbetbody:" + betmsgbody;
                return betmsgbody;
            }
            else if (-1 == betnum.Cfof_check_ball(sball, ref playtype, ref money))
            {
                textBox_test.Text += "\r\n[ERR]:" + "投注号码个数有误";
                return "";
            }
            else if (-3 == betnum.Cfof_check_ball(sball, ref playtype, ref money))
            {
                textBox_test.Text += "\r\n[ERR]:" + "投注号码有重复";
                return "";
            }
            else
            {
                textBox_test.Text += "\r\n[ERR]:" + betnum.Cfof_check_ball(sball, ref playtype, ref money);
                return "";
            }
        }
        #endregion

        //投注 暂未启用 lias投注版本的
        private void Bet_msg(string betbody)
        {
            if (betbody.Length == 0 || betbody == "")
            {

            }
            sequence++;
            string sequenceid = sequence.ToString().PadLeft(6, '0');
            string shead = "1|1|0|" + sequenceid + "|BET|" + AgentId.Text + "|";
            string sbody = betbody;
            string s1 = shead + sbody + sk;
            StringBuilder s2 = new StringBuilder(5120);

            //BzWebSec bzsec = new BzWebSec();//不需要new 因为bzwebsec中这些方法是static
            BzWebSec.WebMD5String32(s1, s2);
            string szy = s2.ToString();
            int stp = -1;
            stp = BzWebSec.WebEncodeString(sbody, sk, s2);
            string msbody = s2.ToString();
            int imsglen = shead.Length + szy.Length + msbody.Length + 1;
            string msglen = "@" + (imsglen.ToString()).PadLeft(4, '0') + "|";
            smsg2 = msglen + shead + msbody + "|" + szy;
            //建socket链接
            if (0 == betsock.Inisocket())
            {
                //发送send
                betsock.Sendmsg(smsg2);
                textBox3.Text += "send message is:" + smsg2 + "\r\n";
                //接受recv
                string srecmsg = betsock.Recvmsg();

                textBox_test.Text += "\r\nreceive message is:" + srecmsg + "\r\n";
                string[] sArray = srecmsg.Split('|');

                string srembody = sArray[7];
                BzWebSec.WebDecodeString(srembody, sk, s2);
                string srebody = s2.ToString();

                textBox_test.Text += "\r\n" + srebody;
            }
            else
            {
                textBox_test.Text += " " + "\r\n" + SocketClass.errstring;
            }

        }

        //bet button 弃用 lias投注版本的
        private void Button3_Click(object sender, EventArgs e)
        {
            string sball = textBox13.Text;
            sendbetstr = Ini_betstr(sball);
            Bet_msg(sendbetstr);
        }

        //关闭窗口事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (false)  //完整版应该为 (_pageNum != 0 ) 调试版本省略确认框 == 0
            {
                if (MessageBox.Show("确认退出程序？", "程序退出确认", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            try
            {
                betsock.Closesock();
            }
            catch
            {
            }
        }

        //信息输出栏定位末行
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Focus();//获取焦点
            textBox3.Select(textBox3.TextLength, 0);//光标定位到文本最后
            textBox3.ScrollToCaret();
        }

        //信息输出栏定位末行
        private void TextBox4_TextChanged(object sender, EventArgs e)
        {

            textBox_test.Focus();//获取焦点
            textBox_test.Select(textBox_test.TextLength, 0);//光标定位到文本最后
            textBox_test.ScrollToCaret(); 

        }

        private void ToolTip1_Popup(object sender, PopupEventArgs e)
        {
        }

        /*
         * 
         * 
         * 
         * */
        #region 号码输入框输入的处理块

        /// <summary>
        /// 满足(TextBox.Text.Length >= 2)，
        /// 焦点移动到下一TabIndex索引的控件 (以后可重载移动条件)
        /// </summary>
        /// <param name="sender"> 控件对象 </param>
        void CheckTextFocus(object sender)
        {
            if (((TextBox)sender).Text.Length >= 2)
            {
                SelectNextControl((Control)this.ActiveControl, true, true, true, false);
                nownumbox = (TextBox)ActiveControl;
                //TestLog("nownumbox = " + nownumbox.Name +"\tfocus to " + ActiveControl.Name);
            }
            else if (((TextBox)sender).Text.Length <= 0 && nownumbox != lstBox.First().First())
            {
                SelectNextControl((Control)this.ActiveControl, false, true, true, false);
                nownumbox = (TextBox)ActiveControl;
                BetNo_Enter(sender, null);
                //TestLog("focus to " + ActiveControl.Name);
            }

        }

        /// <summary>
        /// 所有 BetNo(BetNo_A1 - BetNo_XX) 的 TextChanged 事件都指向该事件实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BetNo_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != string.Empty && Convert.ToInt16(((TextBox)sender).Text) > 32)
            {
                TestLog("选号不能超过32");
            }
            else
            {
                CheckTextFocus(sender);
            }
        }

        /// <summary>
        /// 所有 BetNo(BetNo_A1 - BetNo_XX) 的 Enter 事件都指向该事件实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BetNo_Enter(object sender, EventArgs e)
        {
            nownumbox = (TextBox)sender;
            nownumbox.SelectionStart = nownumbox.Text.Length;
            //nownumbox.Select(nownumbox.TextLength, 0);
        }

        /// <summary>
        /// 键盘事件，界面所有空间注册键盘事件的第一层事件处理
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            //TestLog(Convert.ToInt32(keyData.ToString("D")) +  "\r\n||" + keyData.ToString() + "\r\n||" + keyData);
            int keyvalue = Convert.ToInt32(keyData.ToString("D"));     // Convert.ToInt16(e.KeyChar);
            if ((keyvalue >= 48 && keyvalue <= 57) || ((keyvalue >= 96 && keyvalue <= 105)) || keyvalue == 262162 || keyvalue == 131089 || keyvalue == 65552 || keyData == Keys.Tab)
            {
                return false;
            }
            else     //
            {   //表处理过(即该事件被抛弃，不触发输入,下面再进行具体处理;)
                object keytobtn = new object();
                keytobtn = Convert.ToString(keyData);
                KeyBtnClick(keytobtn, KeyPressEventArgs.Empty);
                return true;
                //TestLog("功能：" + e.KeyChar);
            }
        }

        /// <summary>
        /// 第一层键盘事件不处理的，第二层再处理部分；如退格、方向键等；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PosKeyDown(object sender, KeyEventArgs e)
        {
            //TestLog(e.KeyData.ToString() + e.KeyValue.ToString());
            if ((e.KeyValue >= 48 && e.KeyValue <= 57) || (e.KeyValue >= 96 && e.KeyValue <= 105) || e.KeyValue == 18 || e.KeyValue == 131089 || e.KeyValue == 65552 || e.KeyValue == 9 || e.KeyData == Keys.Left || e.KeyData == Keys.Right || e.KeyData == Keys.Up || e.KeyData == Keys.Down)
            {
                e.Handled = false;
            }
            else// if (PosBack.IsLetter(Convert.ToString(e.KeyCode)))     //
            {
                e.Handled = true;   //表处理过(即该事件被抛弃，不触发输入,下面再进行具体处理;)
                object keytobtn = new object();
                keytobtn = Convert.ToString(e.KeyData);
                KeyBtnClick(keytobtn, KeyPressEventArgs.Empty);
                //TestLog("功能：" + e.KeyChar);
            }
        }

        /// <summary>
        /// 激活主界面时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Activated(object sender, EventArgs e)
        { 
        }

        /// <summary>
        /// 小键盘区域数字点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Num_Click(object sender, EventArgs e)
        {
            string numstr = ((Control)sender).Text;
            
            TestLog("NumClick " + ((Button)sender).Text);
        }

         

        /// <summary>
        /// 功能按键点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyBtnClick(object sender, EventArgs e)
        { 
            string BtnName = sender.GetType() == typeof(string) ? ("Btn" + Convert.ToString(sender)) : ((Control)sender).Name;
            TestLog("调用:" + BtnName);
            //退格BACKSPACE
            if ( BtnName == BtnBack.Name)
            { 
            }
            //ESC
            else if ( BtnName == BtnEscape.Name)
            {
                // 此处调用 ESC 相关
                TestLog("调用:ESC" + BtnName);
            }
            //确定Enter
            else if ( BtnName == BtnEnter.Name)
            {
                // 调用 F8Bet 投注相关
                TestLog("调用: " + BtnName); //Betqueren();
            }
            //F8Bet
            else if (BtnName == BtnF8.Name)
            {
                TestLog( "调用：Bet " + BtnName); 
                
            }
            //
            else if ( BtnName == BtnF9.Name)
            {
                TestLog("调用： " + BtnName);
            }
            else if ( BtnName == BtnA.Name)
            {
                TestLog("调用：  " + BtnName);
                panel_Bet.Visible = true;       //显示投注号码区域
            }
            else if (BtnName == BtnC.Name)
            {
                TestLog("调用：  " + BtnName);
            }
        }

        #endregion

        int fs = 0;     //投注方式，暂时1单式，后面再细化

        /// <summary>
        /// 处理投注的号码；是否合法？写入待发送串；
        /// </summary>
        /// <param name="wf"> 玩法标识，不同玩法的个数不同，组串方式不同</param>
        /// <param name="betnum"> 用于接受组成的字符串 </param>
        int DOF8Bet(int wf,ref string betnum)
        {
            string sendbetnum = string.Empty;
            BetNum betnumclass = new BetNum();

            //取出倍数输入框控件中的值 是空时默认为 1
            string multiple =  "1" ;
            
            int result = betnumclass.MakeBetString(lstBox, multiple, wf, fs);
            string sendstr = betnumclass.sendbetnum;
            //secstr = posback.lstbetnum[(i * (lstcon.Count - 1)) + (i + j)];
            switch (result)
            {
                case 0:
                    if (true)
                    {
                        betsock.Sendmsg(sendstr);
                    }
                    else
                    {
                        betnumclass.AloneBet(wf, fs, multiple);
                    }
                    ;
                    break;
                case -1:
                    TestLog("号码不足");
                    return -1;
                case -2:
                    TestLog("存在重号");
                    return -2;
                default:
                    return -10;
            }


            return 0;
        }


        public void TestLog(string str)
        {
            Console.WriteLine(str);
            textBox_test.Text += str + "\r\n";
        }

        
        /// <summary>
        /// 清空投注区内号码
        /// </summary>
        private void ClearBet()
        {
            foreach (List<TextBox> lstcon in lstBox)
            {
                foreach (Control conl in lstcon)
                {
                    conl.Text = string.Empty;
                }
            }
        }



        #region 初始化号码框

        
        List<List<TextBox>> lstBox = new List<List<TextBox>>();
        int _location_x;    //锚点x
        int _location_y;    //锚点y
        int _count_x;       //单行个数
        int _count_y;       //行数
        int _margin;        //间距
        int _width;         //宽
        int _height;        //高
        
        /// <summary>
        /// 动态初始化投注号码输入框部分，并将每个号码框放入List保存
        /// </summary>
        /// <param name="wf"> 玩法参数，不同玩法的号码框个数不同 </param>
        /// 
        private void CreatBox(int wf)
        {
            _location_x = 40;   
            _location_y = 55;
            _count_x = 0;
            _count_y = 0;
            _width = 25;        
            _height = 20;

            switch (wf)
            {
                case 0:
                    _count_x = 5;
                    _count_y = 5;
                    _margin = 10;
                    break;
                case 1:
                    _count_x = 7;
                    _count_y = 5;
                    _margin = 10;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < _count_y; i++)
            {
                string name1 = (1 + i).ToString();
                List<TextBox> lstText = new List<TextBox>();
                lstBox.Add(lstText);
                for (int j = 0; j < _count_x; j++)
                {
                    if (j == (_count_x - 1) && wf == 1)
                    {
                        _location_x += (9 - 6)*(_width + _margin);
                    }
                    string name2 = (j + 1).ToString();
                    TextBox tbox = new TextBox();
                    panel_Bet.Controls.Add(tbox);
                    tbox.Name = "Bet" + name1 + name2;
                    tbox.MaxLength = 2;
                    tbox.Location = new Point(_location_x, _location_y);
                    tbox.Margin = new Padding(5);
                    tbox.Size = new Size(_width, _height);
                    tbox.TextAlign = HorizontalAlignment.Center;
                    tbox.Visible = true;
                    tbox.TextChanged += new EventHandler(BetNo_TextChanged);
                    tbox.Enter += new EventHandler(BetNo_Enter);
                    tbox.TabStop = true;
                    tbox.TabIndex = i * _count_x + j;// (i * (_count_x - 1)) + (i + j);

                    if (wf == 1 && j == 6)
                    {
                        tbox.Tag = "blue"+ i.ToString();
                    }

                    lstText.Add(tbox);
                    _location_x += _width + _margin;
                    //Console.Write("x" + (j + 1) + ":" + _location_x + "\t");
                }
                _location_x = 40;
                _location_y += _height + _margin;

                //Console.Write("y" + (i + 1) + ":" + _location_y + "\t");
            }
        }

        #endregion

        /// <summary>
        /// 切换选择标签页时，将参数列表数据更新匹配该page，并显示在该page，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            int index = tabControl1.SelectedIndex;
            Console.WriteLine(index);
            Update_panel_Parameters_Show(index);
            ChangePage(index);
            tabControl1.SelectedTab.Controls.Add(panelC515_Parameters);
            tabControl1.SelectedTab.Controls.Add(panel_Bet);
            panelC515_Parameters.Show();
            panel_Bet.Show();
        }

        /// <summary>
        /// 切换TabPage时更新投注区
        /// </summary>
        /// <param name="wf"></param>
        private void ChangePage(int wf)
        {
            switch (wf)
            {
                case 0:
                    for (int i = 0; i < lstBox.Count; i++)
                    {
                        for (int j = 0; j < lstBox[i].Count; j++)
                        {
                            lstBox[i][j].Visible = true;
                            if (j >= 5 )
                            {
                                lstBox[i][j].Visible = false;
                            }
                        }
                    }
                    break;

                case 1:
                    for (int i = 0; i < lstBox.Count; i++)
                    {
                        for (int j = 0; j < lstBox[i].Count; j++)
                        {
                            lstBox[i][j].Visible = true;
                            if (j >= 3)
                            {
                                lstBox[i][j].Visible = false;
                            }
                        }
                    }
                    break;

                case 2:
                    for (int i = 0; i < lstBox.Count; i++)
                    {
                        for (int j = 0; j < lstBox[i].Count; j++)
                        {
                            lstBox[i][j].Visible = true;
                        }
                    }
                    break;

                case 3:
                    for (int i = 0; i < lstBox.Count; i++)
                    {
                        for (int j = 0; j < lstBox[i].Count; j++)
                        {
                            lstBox[i][j].Visible = true;
                            if (j >= 7)
                            {
                                lstBox[i][j].Visible = false;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }            
        }


        private void PaintPicture()
        {
            const int x = 50;
            const int y = 160;
            int locatX = x;
            int locatY = y;
            
            pic1.Visible = true;            
            pic2.Visible = true;
            pic3.Visible = true;
            pic4.Visible = true;
            pic5.Visible = true;
            pic6.Visible = true;
            pic7.Visible = true;
            pic1.Text = "43E00E-7DC398B-000000-000000-00";
            pic2.Text = "站点\0";
            pic3.Text = "特征码 AAAAA00000BBBBB0  654321";
            pic4.Text = "流水号\0";
            pic5.Text = "销售期\0";
            pic6.Text = "兑奖期\0";
            pic7.Text = "金额\0";

            for (int i = 0; i < lstBox.Count; i++)
            {
                for (int j = 0; j < lstBox[i].Count; j++)
                {                    
                    Label lab = new Label();
                    groupBox_LotteryPicture.Controls.Add(lab);
                    if ( groupBox_LotteryPicture.Size.Width - locatX <= 30)
                    {
                        locatX = x;
                        locatY += 40;
                    }
                    if (lstBox[i][j].Tag != null && lstBox[i][j].Tag.ToString() == "blue" + i)
                    {
                        //lab.Text = "+";
                    }
                    lab.Location = new Point(locatX, locatY);
                    lab.Text += lstBox[i][j].Text;
                    lab.BackColor = Color.Transparent;
                    lab.AutoSize = true;
                    locatX += 40;
                    lab.Font =  new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    Console.Write("x" + (j + 1) + ":" + locatX + "\t");
                }
                locatX = x;
                locatY += 30;
                Console.WriteLine("y" + (i + 1) + ":" + locatY + "\t");
            }
        }
    }



}
