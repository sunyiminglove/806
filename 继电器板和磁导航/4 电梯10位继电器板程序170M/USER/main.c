#include "stm32f10x.h"
#include "delay.h"
#include "usart.h"
#include "led.h"
#include "dma.h"
#include "adc.h"
#include "can.h"
#include "Modbus_slave_170M.h"
#include "wdg.h"
float adc1,adc2,adc3,adc4;


int main(void)
{			
	u16 i=0;
	delay_init();	
	NVIC_Configuration();
	uart_init(115200);
	DMA_USART1_Configuration();
		//启动看门狗
	IWDG_Init(4,500);    	//预分频数为64,重载值为500,溢出时间为1s	  

	Init_LEDpin();
	CAN_Mode_Init(CAN_SJW_1tq,CAN_BS2_8tq,CAN_BS1_9tq,4,0);	
	JDQ0 = 1;JDQ1 = 1;JDQ2 = 1;JDQ3 = 1;JDQ4 = 1;JDQ5 = 1;
	JDQ6 = 1;JDQ7 = 1;JDQ8 = 1;JDQ9 = 1;JDQ10 = 1;JDQ11 = 1;
	LED0 = 1;
	send();	
	cansend[2] = 0;
	
	DeviceID = 4;
	while(1)
	{
		//喂狗
		IWDG_Feed();

		PLC2_Data[0] = IN0;
		PLC2_Data[1] = IN1;
		PLC2_Data[2] = IN2;
		PLC2_Data[3] = IN3;		
		PLC2_Data[4] = IN4;		
		PLC2_Data[5] = IN5;
		PLC2_Data[6] = IN6;
		PLC2_Data[7] = IN7;
		PLC2_Data[8] = IN8;		
		PLC2_Data[9] = IN9;
	//	PLC2_Data[10] = IN10;
		
		JDQ0 = PLC2_Data[10];
		JDQ1 = PLC2_Data[11];
		JDQ2 = PLC2_Data[12];
		JDQ3 = PLC2_Data[13];		
		JDQ4 = PLC2_Data[14];		
		JDQ5 = PLC2_Data[15];
		JDQ6 = PLC2_Data[16];
		JDQ7 = PLC2_Data[17];
		JDQ8 = PLC2_Data[18];		
		JDQ9 = PLC2_Data[19];
//		JDQ10 = PLC2_Data[21];		
//		JDQ11 = PLC2_Data[22];
		delay_ms(10);

		i++;
		if(i==30)
		{
			i = 0;
			LED0 = ~LED0;
		}
	}
}


