using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Avance_de_Proyecto_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Se crea un objeto de la clase BDD
        BDD bdd = new BDD();

        private string validarInformacion(ref TextBox Usuario, ref TextBox Pass)
        {
            //Se evalua si el objeto Usuario contiene texto en su propiedad Text
            if (Usuario.Text != "")
            {
                //Se evalua si el objeto Pass contiene texto en su propiedad Text
                if (Pass.Text != "")
                {
                    //Se retorna el resultado del metodo "IniciarSesion" del
                    //objeto "bdd"
                    return bdd.IniciarSesion(Usuario.Text, Pass.Text);
                }
                else
                {
                    //Se muestra un mensaje de error al usuario
                    MessageBox.Show("Debe ingresar su contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Se hace focus al objeto Pass
                    Pass.Focus();
                }
            }
            else
            {
                   //Se muestra un mensaje de error al usuario
                MessageBox.Show("Debe ingresar su usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Se hace focus al objeto Usuario
                Usuario.Focus();
            }

            //Se retorna un string "noMatch"
            return "noMatch";
        }

        //Metodo que se encarga de los procedimientos de inicio de sesion
        private void RedirectLogin()
        {
            //Variable que almacena el tipo de usuario que intenta iniciar sesion
            string cuenta = "";
            //Se asigna el valor que retorne la funcion validarInformacion a la variable cuenta
            cuenta = validarInformacion(ref txtUsuario, ref txtPass);
            //Se verifica si la información ha sido validada correctamente
            if (cuenta == "Empleado")
            {
                //Se crea un objeto llamado f3 de la clase Form3
                Form3 f3 = new Form3(this);
                //Se esconde el presente formulario
                this.Hide();
                //Se  muestra el objeto f3
                f3.Show();
            }
            else if (cuenta == "Administrador")
            {
                //Se crea un objeto llamado f2 de la clase Form2
                Form2 f2 = new Form2(this);
                //Se esconde el presente formulario
                this.Hide();
                //Se  muestra el objeto f2
                f2.Show();
            }
            else if (cuenta == "noMatch")
            {
                //Se muestra un mensaje al usuario
                MessageBox.Show("El usuario o la contraseña ingresada son incorrectos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion "RedirectLogin"
            RedirectLogin();
        }
    }
}