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
	while(1)
	{
		//喂狗
		IWDG_Feed();
		send();
		delay_ms(1);
		i++;
		if(i==500)
		{
			i = 0;
			LED0 = ~LED0;
		}
	}
}


