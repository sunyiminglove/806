/**
  ******************************************************************************
	注意事项：
			1、所用串口为串口6，也就是WIFI的那个串口
			2、配置串口6
			void USART6_Configuration(u32 bound)
			{
				NVIC_InitTypeDef NVIC_InitStructure;
				
				GPIO_InitTypeDef GPIO_InitStructure;
				USART_InitTypeDef USART_InitStructure;
				RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOC,ENABLE); 
				RCC_APB2PeriphClockCmd(RCC_APB2Periph_USART6,ENABLE);
				
				GPIO_PinAFConfig(GPIOC,GPIO_PinSource6,GPIO_AF_USART6);
				GPIO_PinAFConfig(GPIOC,GPIO_PinSource7,GPIO_AF_USART6);
				
				GPIO_InitStructure.GPIO_Pin = GPIO_Pin_6|GPIO_Pin_7;
				GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF;
				GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
				GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
				GPIO_InitStructure.GPIO_PuPd = GPIO_PuPd_UP;
				GPIO_Init(GPIOC,&GPIO_InitStructure);
				
				USART_InitStructure.USART_BaudRate            = bound  ;
				USART_InitStructure.USART_WordLength          = USART_WordLength_8b;
				USART_InitStructure.USART_StopBits            = USART_StopBits_1;
				USART_InitStructure.USART_Parity              = USART_Parity_No ;
				USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;
				USART_InitStructure.USART_Mode                = USART_Mode_Rx | USART_Mode_Tx;
				USART_Init(USART6, &USART_InitStructure);
				USART_ITConfig(USART6, USART_IT_IDLE, ENABLE);	
				USART_Cmd(USART6, ENABLE);  
				USART_ClearFlag(USART6, USART_FLAG_TC);
				USART_DMACmd(USART6, USART_DMAReq_Rx, ENABLE);
				USART_DMACmd(USART6, USART_DMAReq_Tx, ENABLE);//注释掉，用自写u6_printf函数发送
				
				NVIC_InitStructure.NVIC_IRQChannel = USART6_IRQn;  
				NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 2;
				NVIC_InitStructure.NVIC_IRQChannelSubPriority = 2;
				NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
				NVIC_Init(&NVIC_InitStructure);
			}
			3、配置串口6 DMA
			void DMA_Uart6_Init(void)
			{
				NVIC_InitTypeDef NVIC_InitStructure;
				
				DMA_InitTypeDef DMA_InitStructure;  
				
				RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_DMA2, ENABLE);
			 
				DMA_Cmd(DMA2_Stream7, DISABLE);
				DMA_DeInit(DMA2_Stream7);
				DMA_InitStructure.DMA_Channel = DMA_Channel_5;
				DMA_InitStructure.DMA_PeripheralBaseAddr = (uint32_t)(&USART6->DR);
				DMA_InitStructure.DMA_Memory0BaseAddr = (uint32_t)send6_buf;
				DMA_InitStructure.DMA_DIR = DMA_DIR_MemoryToPeripheral;
				DMA_InitStructure.DMA_BufferSize = UART_TX_LEN;
				DMA_InitStructure.DMA_PeripheralInc = DMA_PeripheralInc_Disable;
				DMA_InitStructure.DMA_MemoryInc = DMA_MemoryInc_Enable;
				DMA_InitStructure.DMA_PeripheralDataSize = DMA_PeripheralDataSize_Byte;
				DMA_InitStructure.DMA_MemoryDataSize = DMA_MemoryDataSize_Byte;
				DMA_InitStructure.DMA_Mode = DMA_Mode_Circular;	
				DMA_InitStructure.DMA_FIFOMode = DMA_FIFOMode_Disable; 
				DMA_InitStructure.DMA_FIFOThreshold = DMA_FIFOThreshold_Full;	
				DMA_InitStructure.DMA_Priority = DMA_Priority_VeryHigh;
				DMA_InitStructure.DMA_MemoryBurst = DMA_MemoryBurst_Single;//存储器突发单次传输
				DMA_InitStructure.DMA_PeripheralBurst = DMA_PeripheralBurst_Single;//外设突发单次传输
				DMA_Init(DMA2_Stream7, &DMA_InitStructure);
				DMA_Cmd(DMA2_Stream7, DISABLE);
				DMA_ITConfig(DMA2_Stream7, DMA_IT_TC, ENABLE);				
			 
				DMA_Cmd(DMA2_Stream1, DISABLE);
				DMA_DeInit(DMA2_Stream1);
				DMA_InitStructure.DMA_Channel = DMA_Channel_5;
				DMA_InitStructure.DMA_PeripheralBaseAddr = (uint32_t)(&USART6->DR);
				DMA_InitStructure.DMA_Memory0BaseAddr = (uint32_t)rece6_buf;
				DMA_InitStructure.DMA_DIR = DMA_DIR_PeripheralToMemory ;
				DMA_InitStructure.DMA_BufferSize = UART6_RX_LEN;
				DMA_InitStructure.DMA_PeripheralInc = DMA_PeripheralInc_Disable;
				DMA_InitStructure.DMA_MemoryInc = DMA_MemoryInc_Enable;
				DMA_InitStructure.DMA_PeripheralDataSize = DMA_PeripheralDataSize_Byte;
				DMA_InitStructure.DMA_MemoryDataSize = DMA_MemoryDataSize_Byte;
				DMA_InitStructure.DMA_Mode = DMA_Mode_Circular;
				DMA_InitStructure.DMA_FIFOMode = DMA_FIFOMode_Disable; 
				DMA_InitStructure.DMA_FIFOThreshold = DMA_FIFOThreshold_Full;	
				DMA_InitStructure.DMA_Priority = DMA_Priority_VeryHigh;
				DMA_InitStructure.DMA_MemoryBurst = DMA_MemoryBurst_Single;//存储器突发单次传输
				DMA_InitStructure.DMA_PeripheralBurst = DMA_PeripheralBurst_Single;//外设突发单次传输
				DMA_Init(DMA2_Stream1, &DMA_InitStructure);
				DMA_Cmd(DMA2_Stream1, ENABLE); 
				
				NVIC_InitStructure.NVIC_IRQChannel = DMA2_Stream7_IRQn;  
				NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 2;
				NVIC_InitStructure.NVIC_IRQChannelSubPriority = 4;
				NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
				NVIC_Init(&NVIC_InitStructure);	
			}
			void Uart6_Start_DMA_Tx(u16 size)
			{
				DMA_SetCurrDataCounter(DMA2_Stream7,size);
				DMA_Cmd(DMA2_Stream7, ENABLE);
			}
			4、配置串口6 DMA 发送中断
			void  DMA2_Stream7_IRQHandler(void)//串口6发送DMA中断
			{
				if(DMA_GetITStatus(DMA2_Stream7, DMA_IT_TCIF7))
				{
					DMA_ClearITPendingBit(DMA2_Stream7, DMA_IT_TCIF7);
					DMA_Cmd(DMA2_Stream7, DISABLE);
					send_ok = 1;
				}
			}
			5、配置串口6接收中断
			void USART6_IRQHandler(void)             
			{
				#if SYSTEM_SUPPORT_OS 	
					OSIntEnter();    
				#endif
					if(USART_GetITStatus(USART6, USART_IT_IDLE) != RESET) 
					{
						DMA_Cmd(DMA2_Stream1,DISABLE);
						rece6_index = USART6->SR;
						rece6_index = USART6->DR; //清除IDLE标志
						rece6_index = UART6_RX_LEN - DMA_GetCurrDataCounter(DMA2_Stream1); 
						DMA2_Stream1->NDTR = UART6_RX_LEN;
						receive_ok = 1;
					} 
				#if SYSTEM_SUPPORT_OS 	
					OSIntExit();  											 
				#endif
			} 
			6、将data_Parameterreceive()和Analyze()函数放在两个不同的任务里
			
			void DEMO1_task(void *p_arg)
			{
				while(1)
				{
					if(receive_ok == 1)
					{
						Analyze();
						DMA_Cmd(DMA2_Stream1, ENABLE);
					}		
					delay(0,0,0,10);      
				}
			}
			void DEMO2_task(void *p_arg)
			{
				while(1)
				{
					if(receive_ok == 1)
					{
						data_Parameterreceive();
						DMA_Cmd(DMA2_Stream1, ENABLE);
					}
					delay(0,0,0,10);      
				}
			}
  ******************************************************************************
*/
#include "route.h"
#include "read.h"
#include "dma.h"
#include "string.h"
#include "delay.h"
#include "usart6.h"

void Clear_ReceBuf(u8 num)
{
	memset(send6_buf,0,sizeof(send6_buf));
}	
			
			
			//与参数上位机通讯
//保存路径信息,rout_num:路径号,zhandian_num:站点号
void SetRouteData_pc(u16 rout_num,u16 zhandian_num)
{
	u16 i;
	//保存路径包含站点数
	RouteStationNum[rout_num-1] = zhandian_num;
	//保存
	W25QXX_Write_16(&RouteStationNum[rout_num-1],RouteStationNumAdress(rout_num),1);	

	
	//保存路径站点包含信息
	for(i=0;i<zhandian_num;i++)
	{
		W25QXX_Write_16(&NowRouteInfor[i][0],NowStationInforAdress(rout_num,i),StaiionInfor);	
	}
}
//保存及更新流程数据（当前操作的流程信息）
void SetProcessData_pc(u16 liucheng_nember,u16 xuhao_sum)
{	
	u16 i = 0;
	//流程步数
	
	ProcessStepNum[liucheng_nember-1] = xuhao_sum;
	//保存
	W25QXX_Write_16(&ProcessStepNum[liucheng_nember-1],ProcessStepNumAdress(liucheng_nember),1);	
	
	//保存
	for(i=0;i<xuhao_sum;i++)
		W25QXX_Write_16(&NowProcessInfor[i][0],NowStepInforAdress(liucheng_nember,i),StepInfor);
}
u8 readstate=0,lujing_readstate=0,zhandian_readstate = 0,liucheng_readstate = 0,xuhao_readstate = 0;
	u16 sum,parment_number,lujing_number,i,liucheng_number,zhandian_number,xuhao_number,zhandian_sum,xuhao_sum,rout_number;
void data_Parameterreceive()
{
	u16 temp=0;
	if((rece6_buf[0] == 0xAA)&&(rece6_buf[3] == 0xb4))//PC读取Flash系统参数
	{
		sum = rece6_buf[0]+rece6_buf[1]+rece6_buf[2]+rece6_buf[3];
		if(((sum>>8) ==rece6_buf[4]) &&((sum&0xff) ==rece6_buf[5]) )
		{
			for(i=1;i<501;i++)
			{
				if(GetRouteStationNum(i))	
					temp++;
			}
			SystemParameter[99] = temp;
			temp = 0;
			for(i=1;i<501;i++)
			{
				if(GetProcessStepNum(i))	
					temp++;
			}
			SystemParameter[98] = temp;			
			send6_buf[0] = 0xAA;send6_buf[1] = 0xAA;//帧头			
			receive_ok=0;
			send_ok = 0;
			
			send6_buf[2] = 0;
			send6_buf[3] = 10;			
			send6_buf[4] = rece6_buf[1];
			send6_buf[5] = rece6_buf[2];
			parment_number = rece6_buf[1] << 8 | rece6_buf[2];
			send6_buf[6] = SystemParameter[parment_number]>>8;
			send6_buf[7] = SystemParameter[parment_number]&0xff;
			sum = send6_buf[0]+send6_buf[1]+send6_buf[2]+send6_buf[3]+send6_buf[4]+send6_buf[5]
			+send6_buf[6]+send6_buf[7];
			send6_buf[8] = sum>>8;send6_buf[9] = sum&0xff;
			Uart6_Start_DMA_Tx(10);
			while(send_ok == 0)
			{
				delay_ms(2);
			}
			Clear_ReceBuf(6);			
		}
	}
	else if((rece6_buf[0] == 0xBB)&&(rece6_buf[3] == 0xb4))//PC读取Flash路径
	{
		sum = rece6_buf[0]+rece6_buf[1]+rece6_buf[2]+rece6_buf[3];
		if(((sum>>8) ==rece6_buf[4]) &&((sum&0xff) ==rece6_buf[5]) )
		{
			send6_buf[0] = 0xAA;send6_buf[1] = 0xBB;//帧头
			receive_ok=0;
			send_ok = 0;
			send6_buf[2] = 0;
			send6_buf[3] = 31;
			
			send6_buf[4] = rece6_buf[1];
			send6_buf[5] = rece6_buf[2];
			
			lujing_number = rece6_buf[1] << 8 | rece6_buf[2];
			HmiRouteNum = lujing_number;
			GetRouteData(lujing_number);
			if(HmiStationNum>0)
				HmiStationSerialNum = 1;
			UpdataStationToHmi();
			send6_buf[6] = RouteStationNum[lujing_number-1]>>8;
			send6_buf[7] = RouteStationNum[lujing_number-1]&0xff;		
			if(RouteStationNum[lujing_number-1]!=0)
			{
				for(i=0;i<RouteStationNum[lujing_number-1];i++)
				{
					send_ok = 0;
					send6_buf[8] = NowRouteInfor[i][0];
					send6_buf[9] = NowRouteInfor[i][1];
					send6_buf[10] = NowRouteInfor[i][2];
					send6_buf[11] = NowRouteInfor[i][3];
					send6_buf[12] = NowRouteInfor[i][4];
					send6_buf[13] = NowRouteInfor[i][5]>>8;
					send6_buf[14] = NowRouteInfor[i][5]&0xff;
					send6_buf[15] = NowRouteInfor[i][6];
					send6_buf[16] = NowRouteInfor[i][7];
					send6_buf[17] = NowRouteInfor[i][8];
					send6_buf[18] = NowRouteInfor[i][9];
					send6_buf[19] = NowRouteInfor[i][10];
					send6_buf[20] = NowRouteInfor[i][11];
					send6_buf[21] = NowRouteInfor[i][12];
					send6_buf[22] = NowRouteInfor[i][13];
					send6_buf[23] = NowRouteInfor[i][14];
					send6_buf[24] = NowRouteInfor[i][15];
					send6_buf[25] = NowRouteInfor[i][16];
					send6_buf[26] = NowRouteInfor[i][17];
					send6_buf[27] = NowRouteInfor[i][18];
					send6_buf[28] = NowRouteInfor[i][19];
					
					sum = send6_buf[0]+send6_buf[1]+send6_buf[2]+send6_buf[3]+send6_buf[4]+send6_buf[5]
					+send6_buf[6]+send6_buf[7]+send6_buf[8]+send6_buf[9]+send6_buf[10]+send6_buf[11]
					+send6_buf[12]+send6_buf[13]+send6_buf[14]+send6_buf[15]+send6_buf[16]+send6_buf[17]
					+send6_buf[18]+send6_buf[19]+send6_buf[20]+send6_buf[21]+send6_buf[22]+send6_buf[23]
					+send6_buf[24]+send6_buf[25]+send6_buf[26]+send6_buf[27]+send6_buf[28];
					
					send6_buf[29] = sum>>8;send6_buf[30] = sum&0xff;
					Uart6_Start_DMA_Tx(31);
					while(send_ok == 0)
					{
						delay_ms(2);
					}
					Clear_ReceBuf(6);
				}			
			}
			else
			{
				send6_buf[8] = 0;
				send6_buf[9] = 0;
				send6_buf[10] = 0;
				send6_buf[11] = 0;
				send6_buf[12] = 0;
				send6_buf[13] = 0;
				send6_buf[14] = 0;
				send6_buf[15] = 0;
				send6_buf[16] = 0;
				send6_buf[17] = 0;
				send6_buf[18] = 0;
				send6_buf[19] = 0;
				send6_buf[20] = 0;
				send6_buf[21] = 0;
				send6_buf[22] = 0;
				send6_buf[23] = 0;
				send6_buf[24] = 0;
				send6_buf[25] = 0;
				send6_buf[26] = 0;
				send6_buf[27] = 0;
				send6_buf[28] = 0;
				
				sum = send6_buf[0]+send6_buf[1]+send6_buf[2]+send6_buf[3]+send6_buf[4]+send6_buf[5]
				+send6_buf[6]+send6_buf[7]+send6_buf[8]+send6_buf[9]+send6_buf[10]+send6_buf[11]
				+send6_buf[12]+send6_buf[13]+send6_buf[14]+send6_buf[15]+send6_buf[16]+send6_buf[17]
				+send6_buf[18]+send6_buf[19]+send6_buf[20]+send6_buf[21]+send6_buf[22]+send6_buf[23]
				+send6_buf[24]+send6_buf[25]+send6_buf[26]+send6_buf[27]+send6_buf[28];
				
				send6_buf[29] = sum>>8;send6_buf[30] = sum&0xff;
				Uart6_Start_DMA_Tx(31);
				while(send_ok == 0)
				{
					delay_ms(2);
				}
				Clear_ReceBuf(6);
			}
		}
	}
	else if((rece6_buf[0] == 0xEE)&&(rece6_buf[3] == 0xb4))//PC读取Flash流程
	{
		sum = rece6_buf[0]+rece6_buf[1]+rece6_buf[2]+rece6_buf[3];
		if(((sum>>8) ==rece6_buf[4]) &&((sum&0xff) ==rece6_buf[5]) )
		{
			send6_buf[0] = 0xAA;send6_buf[1] = 0xEE;//帧头
			receive_ok=0;
			send_ok = 0;
			send6_buf[2] = 0;
			send6_buf[3] = 16;
			
			send6_buf[4] = rece6_buf[1];
			send6_buf[5] = rece6_buf[2];
			
			liucheng_number = rece6_buf[1] << 8 | rece6_buf[2];
			HmiProcessNum = liucheng_number;
			GetProcessData();						
			UpdataProcessToHmi();
			
			send6_buf[6] = ProcessStepNum[liucheng_number-1]>>8;
			send6_buf[7] = ProcessStepNum[liucheng_number-1]&0xff;		
			if(ProcessStepNum[liucheng_number-1]!=0)
			{
				for(i=0;i<ProcessStepNum[liucheng_number-1];i++)
				{
					send_ok = 0;					
					send6_buf[8] = NowProcessInfor[i][0]>>8;
					send6_buf[9] = NowProcessInfor[i][0]&0xff;
					send6_buf[10] = NowProcessInfor[i][1]>>8;
					send6_buf[11] = NowProcessInfor[i][1]&0xff;
					send6_buf[12] = NowProcessInfor[i][2]>>8;
					send6_buf[13] = NowProcessInfor[i][2]&0xff;
					sum = send6_buf[0]+send6_buf[1]+send6_buf[2]+send6_buf[3]+send6_buf[4]+send6_buf[5]
					+send6_buf[6]+send6_buf[7]+send6_buf[8]+send6_buf[9]+send6_buf[10]+send6_buf[11]+send6_buf[12]
					+send6_buf[13];
					
					send6_buf[14] = sum>>8;send6_buf[15] = sum&0xff;
					Uart6_Start_DMA_Tx(16);
					while(send_ok == 0)
					{
						delay_ms(2);
					}
					Clear_ReceBuf(6);
				}			
			}
			else
			{
				send6_buf[8] = 0;
				send6_buf[9] = 0;
				send6_buf[10] = 0;
				send6_buf[11] = 0;
				send6_buf[12] = 0;
				send6_buf[13] = 0;				
				sum = send6_buf[0]+send6_buf[1]+send6_buf[2]+send6_buf[3]+send6_buf[4]+send6_buf[5]
				+send6_buf[6]+send6_buf[7]+send6_buf[8]+send6_buf[9]+send6_buf[10]+send6_buf[11]+send6_buf[12]
				+send6_buf[13];
				
				send6_buf[14] = sum>>8;send6_buf[15] = sum&0xff;
				Uart6_Start_DMA_Tx(16);
				while(send_ok == 0)
				{
					delay_ms(2);
				}
				Clear_ReceBuf(6);
			}
		}
	}
	else if((rece6_buf[0] == 0xAA)&&(rece6_buf[1] == 0xAA))//PC写入系统参数到Flash
	{
		receive_ok=0;
		sum = rece6_buf[0]+rece6_buf[1]+rece6_buf[2]+rece6_buf[3]+rece6_buf[4]+rece6_buf[5];
		if(((sum>>8) ==rece6_buf[6]) &&((sum&0xff) ==rece6_buf[7]) )
		{
				parment_number = rece6_buf[2] << 8 | rece6_buf[3];
				SystemParameter[parment_number] = rece6_buf[4] << 8 | rece6_buf[5];
				send_ok = 0;
				send6_buf[0] = 0xAA;//帧头
				send6_buf[1] = 0xCC;//帧头
				send6_buf[2] = 0;
				send6_buf[3] = 10;
				send6_buf[4] = parment_number>>8;
				send6_buf[5] = parment_number&0xff;
				send6_buf[6] = rece6_buf[4];
				send6_buf[7] = rece6_buf[5];				
				sum = send6_buf[0]+send6_buf[1]+send6_buf[2]+send6_buf[3]+send6_buf[4]+send6_buf[5]+send6_buf[6]+send6_buf[7];
				send6_buf[8] = sum>>8;send6_buf[9] = sum&0xff;
				
			
				//存储所有系统参数
				if(parment_number == 99)
				{
					SetAllParameterToSystem();
					UserConfigInit();				
				}			

				Uart6_Start_DMA_Tx(10);
				while(send_ok == 0)
				{
					delay_ms(2);
				}
		}
		Clear_ReceBuf(6);			
		
	}		
	else if((rece6_buf[0] == 0xAA)&&(rece6_buf[1] == 0xbb))////PC写入路径参数到Flash
	{
		receive_ok=0;
		sum = rece6_buf[0]+rece6_buf[1]+rece6_buf[2]+rece6_buf[3]+rece6_buf[4]+rece6_buf[5]
			+rece6_buf[6]+rece6_buf[7]+rece6_buf[8]+rece6_buf[9]+rece6_buf[10]+rece6_buf[11]
			+rece6_buf[12]+rece6_buf[13]+rece6_buf[14]+rece6_buf[15]+rece6_buf[16]+rece6_buf[17]+rece6_buf[18]
		+rece6_buf[19]+rece6_buf[20]+rece6_buf[21]+rece6_buf[22]+rece6_buf[23]+rece6_buf[24]+rece6_buf[25]
		+rece6_buf[26]+rece6_buf[27]+rece6_buf[28];
		if(((sum>>8) ==rece6_buf[29]) &&((sum&0xff) ==rece6_buf[30]))
		{
			rout_number = rece6_buf[2] << 8 | rece6_buf[3];
			zhandian_sum = rece6_buf[4] << 8 | rece6_buf[5];
			zhandian_number = rece6_buf[6] << 8 | rece6_buf[7];
			
			NowRouteInfor[zhandian_number][0] = rece6_buf[8];
			NowRouteInfor[zhandian_number][1] = rece6_buf[9];
			NowRouteInfor[zhandian_number][2] = rece6_buf[10];
			NowRouteInfor[zhandian_number][3] = rece6_buf[11];
			NowRouteInfor[zhandian_number][4] = rece6_buf[12];
			NowRouteInfor[zhandian_number][5] = (rece6_buf[13]<<8)|(rece6_buf[14]&0xff);
			NowRouteInfor[zhandian_number][6] = rece6_buf[15];
			NowRouteInfor[zhandian_number][7] = rece6_buf[16];
			NowRouteInfor[zhandian_number][8] = rece6_buf[17];	
			NowRouteInfor[zhandian_number][9] = rece6_buf[18];
			NowRouteInfor[zhandian_number][10] = rece6_buf[19];
			NowRouteInfor[zhandian_number][11] = rece6_buf[20];
			
			NowRouteInfor[zhandian_number][12] = rece6_buf[21];	
			NowRouteInfor[zhandian_number][13] = rece6_buf[22];
			NowRouteInfor[zhandian_number][14] = rece6_buf[23];
			NowRouteInfor[zhandian_number][15] = rece6_buf[24];
			NowRouteInfor[zhandian_number][16] = rece6_buf[25];	
			NowRouteInfor[zhandian_number][17] = rece6_buf[26];
			NowRouteInfor[zhandian_number][18] = rece6_buf[27];
			NowRouteInfor[zhandian_number][19] = rece6_buf[28];
			
			send_ok = 0;
			send6_buf[0] = 0xAA;//帧头
			send6_buf[1] = 0xdd;//帧头
			send6_buf[2] = 0;
			send6_buf[3] = 12;
			send6_buf[4] = rece6_buf[2];
			send6_buf[5] = rece6_buf[3];
			send6_buf[6] = rece6_buf[4];
			send6_buf[7] = rece6_buf[5];
			send6_buf[8] = rece6_buf[6];
			send6_buf[9] = rece6_buf[7];
			sum = send6_buf[0]+send6_buf[1]+send6_buf[2]+send6_buf[3]+send6_buf[4]+send6_buf[5]
			+send6_buf[6]+send6_buf[7]+send6_buf[8]+send6_buf[9];
			send6_buf[10] = sum>>8;send6_buf[11] = sum&0xff;	
			
			if(zhandian_number == (zhandian_sum-1))
			{
				SetRouteData_pc(rout_number,zhandian_sum);
			}
			
			Uart6_Start_DMA_Tx(12);			
			
			while(send_ok == 0)
			{
				delay_ms(2);
			}
		}	
		Clear_ReceBuf(6);	
	}
	else if((rece6_buf[0] == 0xAA)&&(rece6_buf[1] == 0xee))////PC写入流程参数到Flash
	{
		receive_ok=0;
		sum = rece6_buf[0]+rece6_buf[1]+rece6_buf[2]+rece6_buf[3]+rece6_buf[4]+rece6_buf[5]
			+rece6_buf[6]+rece6_buf[7]+rece6_buf[8]+rece6_buf[9]+rece6_buf[10]+rece6_buf[11]
			+rece6_buf[12]+rece6_buf[13];
		if(((sum>>8) ==rece6_buf[14]) &&((sum&0xff) ==rece6_buf[15]) )
		{
			liucheng_number = rece6_buf[2] << 8 | rece6_buf[3];
			xuhao_sum = rece6_buf[4] << 8 | rece6_buf[5];
			xuhao_number = rece6_buf[6] << 8 | rece6_buf[7];	
			
			NowProcessInfor[xuhao_number][0] = rece6_buf[8]<<8|rece6_buf[9]&0xff;
			NowProcessInfor[xuhao_number][1] = rece6_buf[10]<<8|rece6_buf[11]&0xff;
			NowProcessInfor[xuhao_number][2] = rece6_buf[12]<<8|rece6_buf[13]&0xff;
			
			send_ok = 0;
			send6_buf[0] = 0xAA;//帧头
			send6_buf[1] = 0x55;//帧头
			send6_buf[2] = 0;
			send6_buf[3] = 12;
			send6_buf[4] = rece6_buf[2];
			send6_buf[5] = rece6_buf[3];
			send6_buf[6] = rece6_buf[4];
			send6_buf[7] = rece6_buf[5];
			send6_buf[8] = rece6_buf[6];
			send6_buf[9] = rece6_buf[7];
			sum = send6_buf[0]+send6_buf[1]+send6_buf[2]+send6_buf[3]+send6_buf[4]+send6_buf[5]
			+send6_buf[6]+send6_buf[7]+send6_buf[8]+send6_buf[9];
			send6_buf[10] = sum>>8;send6_buf[11] = sum&0xff;
			if(xuhao_number == (xuhao_sum-1))
			{
				SetProcessData_pc(liucheng_number,xuhao_sum);
			}				
			Uart6_Start_DMA_Tx(12);
			while(send_ok == 0)
			{
				delay_ms(2);
			}
		}
		Clear_ReceBuf(6);					
	}
}
