#include "stm32f10x.h"
#include "delay.h"
#include "usart.h"
#include "led.h"
#include "dma.h"
#include "adc.h"
#include "can.h"
#include "Modbus_slave_170M.h"
#include "wdg.h"

int main(void)
{			
	delay_init();	
	NVIC_Configuration();
//	uart_init(9600);
//	DMA_USART1_Configuration();
	
		//启动看门狗
	IWDG_Init(4,500);    	//预分频数为64,重载值为500,溢出时间为1s	  
	Init_LEDpin();
	CAN_Mode_Init(CAN_SJW_1tq,CAN_BS2_8tq,CAN_BS1_9tq,4,0);	
	
	deviceId = 3;
	
	while(1)
	{
		//喂狗
		IWDG_Feed();
		send();
		delay_ms(10);
	}
}


