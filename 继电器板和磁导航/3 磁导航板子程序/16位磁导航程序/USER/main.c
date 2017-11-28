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
	Init_LEDpin();
	CAN_Mode_Init(CAN_SJW_1tq,CAN_BS2_8tq,CAN_BS1_9tq,4,0);	
			//�������Ź�
	IWDG_Init(4,500);    	//Ԥ��Ƶ��Ϊ64,����ֵΪ500,���ʱ��Ϊ1s	  

	while(1)
	{
		//ι��
		IWDG_Feed();
		send();
		delay_ms(10);
	}
}


