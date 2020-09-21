using System;
using System.Windows.Forms;

namespace MHW_Defense_Calculator {
    public partial class Form1 : Form {
        public Form1( ) {
            InitializeComponent( );
        }

        //Suppress1 is used to skip the DefenseValueChanged event so that the two events don't call each other indefinitely.
        //Suppress2 is used for much the same.
        private bool Suppress1, Suppress2;      
        
        //Called when the textbox in the left side of the equation is changed.
        private void DefenseValueChanged( object sender, EventArgs e ) {
            //Try/Catch to ignore invalid inputs.
            try {
                //If the flag to skip the event is called, unset it and return.
                if ( Suppress1 ) {
                    Suppress1 = false;
                    return;
                }
                //Set the flag to skip the other textbox's event.
                Suppress2 = true;

                //     80
                //------------  = Multiplier for Incoming Physical Damage (MIPD)
                //80 + Defense
                //Source: https://monsterhunterworld.wiki.fextralife.com/Defense

                //Percentage Defense Reduction (PDR) = (1 - MIPD) * 100;

                //The lower portion of the fraction is 80 plus our input defense value.
                double LoPart = 80.0 + double.Parse( textBox1.Text );

                //The rational value is then calculated, and is in the range of 0.0 <= x <= 1.0 with input defense values from the game.
                //We could limit the input for defense to 0 <= x <= infinity, but in normal use it still works correctly, so it's 
                //just interesting to see what would happen if our defense was negative. In those cases, the values can be anywhere.
                double RawDamageMultiplier = 80.0 / LoPart;

                //We then calculate the value of 1 - RawDamageMultiplier to turn it into our reduction percent. 
                double RDMComplement = 1.0 - RawDamageMultiplier;
                
                //We then have to multiply our Raw Damage Multiplier Complement by 100 to get it to be a normal percentage.
                double PercentRepresentation = RDMComplement * 100.0;

                //We update the second text box with the percentage representation truncated to 1 decimal place.
                textBox2.Text = PercentRepresentation.ToString( "n1" );
            }
            //We don't really need to do anything on an invalid input, we're just stopping a dialog from opening.
            catch { };

        }

        private void PercentageDefenseChanged( object sender, EventArgs e ) {
            //Try/Catch to ignore invalid inputs.
            try {
                //If the flag to skip the event is called, unset it and return.
                if ( Suppress2 ) {
                    Suppress2 = false;
                    return;
                }
                //Set the flag to skip the first textbox's event.
                Suppress1 = true;

                //Raw Defense = (80.0 / Raw Damage Multiplier) - 80.0

                //PROOF:
                //D = 80 / (80 + X) where X = raw defense value and D = raw damage multiplier
                //D(80 + X) = 80
                //DX + 80D = 80
                //DX = 80 - 80D
                //X = (80 / D) - 80

                //Get the raw value for our percentage from the textbox.
                double PercentageRepresentation = double.Parse( textBox2.Text );

                //Calculate the raw damage multiplier percentage
                double RDMPercent = 100.0 - PercentageRepresentation;

                //Calculate the real raw damage multiplier.
                double RawDamageMultiplier = RDMPercent / 100.0;

                //Calculate the real value of the input defense.
                double Value = (80.0 / RawDamageMultiplier) - 80.0;

                //Input the defense, cutting off all decimal places.
                textBox1.Text = Value.ToString( "n0" );
            }
            //We don't really need to do anything on an invalid input, we're just stopping a dialog from opening.
            catch { }
        }
    }
}
