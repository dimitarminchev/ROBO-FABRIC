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
using Gadgeteer.Modules.GHIElectronics;

namespace FEZ_Cerb___Rover
{
    public partial class Program
    {
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            /*******************************************************************************************
            Modules added in the Program.gadgeteer designer view are used by typing 
            their name followed by a period, e.g.  button.  or  camera.
            
            Many modules generate useful events. Type +=<tab><tab> to add a handler to an event, e.g.:
                button.ButtonPressed +=<tab><tab>
            
            If you want to do something periodically, use a GT.Timer and handle its Tick event, e.g.:
                GT.Timer timer = new GT.Timer(1000); // every second (1000ms)
                timer.Tick +=<tab><tab>
                timer.Start();
            *******************************************************************************************/


            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");

            // Pinouts
            GT.SocketInterfaces.DigitalOutput P3 = extender.CreateDigitalOutput(GT.Socket.Pin.Three, false);
            GT.SocketInterfaces.DigitalOutput P4 = extender.CreateDigitalOutput(GT.Socket.Pin.Four, false);
            GT.SocketInterfaces.DigitalOutput P5 = extender.CreateDigitalOutput(GT.Socket.Pin.Five, false);
            GT.SocketInterfaces.DigitalOutput P6 = extender.CreateDigitalOutput(GT.Socket.Pin.Six, false);

            bool[] Arr1 = new bool[] {true,false,true,false,false,false,true,true,false,true,false,true,false,false,false};
            bool[] Arr2 = new bool[] { true,false,false,true,false,false,false,false,true,true,true,false,false,true,false};


            for (int i = 0; i < Arr1.Length; i++)
            {
                P4.Write(Arr1[i]);
                P5.Write(Arr2[i]);
                Thread.Sleep(3000);
            }



        }

    }
}
