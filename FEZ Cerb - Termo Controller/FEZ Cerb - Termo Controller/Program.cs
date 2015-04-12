using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace FEZ_Cerb___Termo_Controller
{
    public partial class Program
    {
        // Numbers
        static public byte[] Number0 = new byte[] { 126, 66, 66, 66, 66, 66, 66, 126 };
        static public byte[] Number1 = new byte[] { 8, 24, 40, 8, 8, 8, 8, 8 };
        static public byte[] Number2 = new byte[] { 126, 2, 2, 126, 64, 64, 64, 126 };
        static public byte[] Number3 = new byte[] { 124, 2, 2, 126, 2, 2, 2, 124 };
        static public byte[] Number4 = new byte[] { 2, 4, 8, 16, 34, 66, 254, 2 };
        static public byte[] Number5 = new byte[] { 126, 64, 64, 124, 2, 2, 2, 124 };
        static public byte[] Number6 = new byte[] { 126, 64, 64, 126, 66, 66, 66, 126 };
        static public byte[] Number7 = new byte[] { 126, 2, 2, 4, 8, 16, 32, 64 };
        static public byte[] Number8 = new byte[] { 60, 66, 66, 60, 66, 66, 66, 60 };
        static public byte[] Number9 = new byte[] { 126, 66, 66, 126, 2, 2, 2, 124 };

        // Modules
        private Gadgeteer.Modules.GHIElectronics.LEDMatrix LED1;
        private Gadgeteer.Modules.GHIElectronics.LEDMatrix LED2;
        private Gadgeteer.Modules.GHIElectronics.Thermocouple Thermocouple;
        private Gadgeteer.Modules.GHIElectronics.RelayISOx16 Relay;
        private Gadgeteer.Modules.GHIElectronics.Potentiometer Potentiometer;

        // Temp
        private int P1 = 0, P2 = 0, HIST = 9;        

        // Timers
        private GT.Timer TempTimer       = new GT.Timer(1000); 
        private GT.Timer RelayStartTimer = new GT.Timer(3000); 
        private GT.Timer RelayStopTimer  = new GT.Timer(5000); 

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            // modules
            this.LED1 = new GTM.GHIElectronics.LEDMatrix(7);
            this.LED2 = new GTM.GHIElectronics.LEDMatrix(this.LED1.DaisyLinkSocketNumber);
            this.Thermocouple = new GTM.GHIElectronics.Thermocouple(6);
            this.Relay = new GTM.GHIElectronics.RelayISOx16(4);
            this.Potentiometer = new GTM.GHIElectronics.Potentiometer(3);
            this.Relay.DisableAllRelays();

            // Temp Timer           
            TempTimer.Tick += TempTimer_Tick;
            TempTimer.Start();

            // Timer            
            RelayStartTimer.Tick += RelayStartTimer_Tick;
            RelayStopTimer.Tick += RelayStopTimer_Tick;
        }
        

        // Relay Start Timer
        void RelayStartTimer_Tick(GT.Timer timer)
        {
            Relay.DisableRelay(2);
            Relay.DisableRelay(4);

            RelayStartTimer.Stop();
            RelayStopTimer.Start();
        }

        // Relay Stop Timer
        void RelayStopTimer_Tick(GT.Timer timer)
        {
            RelayStopTimer.Stop();
        }

        // TempTimer
        void TempTimer_Tick(GT.Timer timer)
        {
            P1 = (int)(Potentiometer.ReadPotentiometerPercentage()*100) + 180;
            Debug.Print("Potentiometer: " +P1);

            if (P1 != P2)
            {
                var D3 = (P1 / 100) % 10;
                var D2 = (P1 / 10) % 10;
                var D1 = (P1 / 1) % 10;
                SelectDigit1(D3);
                SelectDigit2(D2);
            }
            else
            {
                // var T1 = Thermocouple.GetInternalTemp_Celsius();
                var T2 = Thermocouple.GetExternalTemp_Celsius();
                Debug.Print("External: "+T2); 
               
                var D3 = (T2 / 100) % 10;
                var D2 = (T2 / 10) % 10;
                var D1 = (T2 / 1) % 10;
                SelectDigit1(D3);
                SelectDigit2(D2);

                // Temperature Control
                if (T2 < P1 - HIST && !RelayStopTimer.IsRunning)
                {
                    Relay.EnableRelay(2);
                    Relay.EnableRelay(4);
                    RelayStartTimer.Start();
                    
                }
                if (T2 > P1 + HIST)
                {
                    Relay.DisableRelay(2);
                    Relay.DisableRelay(4);
                }
            }


            P2 = P1;
        }

        // Select Digit
        void SelectDigit1(int number)
        {
            switch (number)
            {
                case 0: LED1.DrawBitmap(Number0); break;
                case 1: LED1.DrawBitmap(Number1); break;
                case 2: LED1.DrawBitmap(Number2); break;
                case 3: LED1.DrawBitmap(Number3); break;
                case 4: LED1.DrawBitmap(Number4); break;
                case 5: LED1.DrawBitmap(Number5); break;
                case 6: LED1.DrawBitmap(Number6); break;
                case 7: LED1.DrawBitmap(Number7); break;
                case 8: LED1.DrawBitmap(Number8); break;
                case 9: LED1.DrawBitmap(Number9); break;
            }
        }
        void SelectDigit2(int number)
        {
            switch (number)
            {
                case 0: LED2.DrawBitmap(Number0); break;
                case 1: LED2.DrawBitmap(Number1); break;
                case 2: LED2.DrawBitmap(Number2); break;
                case 3: LED2.DrawBitmap(Number3); break;
                case 4: LED2.DrawBitmap(Number4); break;
                case 5: LED2.DrawBitmap(Number5); break;
                case 6: LED2.DrawBitmap(Number6); break;
                case 7: LED2.DrawBitmap(Number7); break;
                case 8: LED2.DrawBitmap(Number8); break;
                case 9: LED2.DrawBitmap(Number9); break;
            }
        }
    }
}
