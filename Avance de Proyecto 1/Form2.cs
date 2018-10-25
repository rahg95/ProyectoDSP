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
    public partial class Form2 : Form
    {
        //Se crea un objeto de clase "Form1"
        Form1 f1;
        //Variable que indica si se va a cerrar el form por cerrar sesion o por la X
        public bool CerrarSesion = false;

        public Form2(Form1 f1)
        {
            InitializeComponent();
            //Se crea una copia del parametro "f1" y se almacena en el objeto de la clase "f1"
            this.f1 = f1;
        }

        //Se crea un objeto de clase "BDD"
        BDD bdd = new BDD();
        //Lista de Clase "TabPage"
        TabPage[] Paginas = new TabPage[7];
        //Variable que lleva la cuenta de la cantidad de productos ingresados dentro de la base de datos
        int cantProductos = 0;

        //Funcion que se encarga de guardar todas los objetos "TabPage" del objeto "tabContenedor"
        //dentro de un arreglo de tipo "TabPage"
        private void AlmacenarPaginas(ref TabPage[] tabs)
        {
            //Se crea una copia de cada objeto de clase "TabPage" contenido dentro del arreglo
            //"TabPages" del objeto "tabContenedor" y se almacena en cada uno de los indices
            //del arreglo "tabs"
            tabs[0] = tabContenedor.TabPages[0];
            tabs[1] = tabContenedor.TabPages[1];
            tabs[2] = tabContenedor.TabPages[2];
            tabs[3] = tabContenedor.TabPages[3];
            tabs[4] = tabContenedor.TabPages[4];
            tabs[5] = tabContenedor.TabPages[5];
            tabs[6] = tabContenedor.TabPages[6];
        }

        //Funcion que se encarga de limpiar los objetos "TabPage" dentro de la coleccion "TabPages"
        //del objeto "tabContenedor" para luego agregar la pagina que se desea visualizar, la cual
        //está especificada haciendo referencia a su índice en el parametro "destino"
        public void TabSwitch(int destino)
        {
            //Se limpian todos los objetos "TabPage" dentro de la coleccion "TabPages"
            //del objeto "tabContenedor"
            tabContenedor.TabPages.Clear();
            //Se crea un nuevo objeto "TabPage" dentro del arreglo "TabPages" del objeto "tabContenedor"
            tabContenedor.TabPages.Add(Paginas[destino]);
        }

        //Funcion que se encarga de limpiar el contenido de ciertos objetos en concreto
        private void LimpiarCampos()
        {
            //Se limpia el contenido del objeto "txtNomProducto"
            txtNomProducto.Text = "";
            //Se limpia el contenido del objeto "txtCodProducto"
            txtCodProducto.Text = "";
            //Se limpia el contenido del objeto "txtLabProducto"
            txtLabProducto.Text = "";
            //Se selecciona el indice "0" del objeto "cboForma"
            cboForma.SelectedIndex = 0;
            //Se limpia el contenido del objeto "txtPresentacion"
            txtPresentacion.Text = "";
            //Se selecciona el indice "0" del objeto "cboTipoMedicamento"
            cboTipoMedicamento.SelectedIndex = 0;
            //Se establece en "0" el valor del objeto "numPrecioCompra"
            numPrecioCompra.Value = 0;
            //Se establece en "0" el valor del objeto "numPrecioVenta"
            numPrecioVenta.Value = 0;
            //Se establece en "0" el valor del objeto "numUnidadesIniciales"
            numUnidadesIniciales.Value = 0;
        }

        //Funcion que se encarga de cerrar sesion
        private bool CS()
        {
            if (MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                //Se muestra el objeto "f1"
                f1.Show();
                //Se cambia el valor de la variable "CerrarSesion"
                CerrarSesion = true;
                //Se limpia el contenido del objeto "txtUsuario" del objeto "f1"
                f1.txtUsuario.Text = "";
                //Se limpia el contenido del objeto "txtPass" del objeto "f1"
                f1.txtPass.Text = "";
                //Se selecciona el objeto "txtUsuario" del objeto "f1"
                f1.txtUsuario.Select();
                //Se cierra el Form actual
                this.Close();
                //Se retorna true
                return true;
            }
            //Se retorna false
            return false;
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //Se llama a la funcion "AlmacenarPaginas"
            AlmacenarPaginas(ref Paginas);
            //Se limpia el contenido de la coleccion "TabPages" del objeto "TabContenedor"
            tabContenedor.TabPages.Clear();
            //Se llama a la funcion TabSwtich
            TabSwitch(0);
            //Se selecciona el objeto "btnAddProduct"
            btnAddProduct.Select();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(1);
            //Se selecciona el objeto "txtNomProducto"
            txtNomProducto.Select();
            //Se llama a la funcion "LimpiarCampos"
            LimpiarCampos();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (bdd.mostrarProducto(ref cboProducto))
            {
                //Se llama a la funcion TabSwtich
                TabSwitch(2);
                //Se selecciona el objeto "cboProducto"
                cboProducto.Select();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(3);
            //Se llama a la funcion mostrarProducto del objeto "bdd"
            bdd.mostrarProducto(ref cmbProducto);
            //Se selecciona el objeto "cmbProducto"
            cmbProducto.Select();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            dgvInventario.DataSource = bdd.mostrarInventario();
            //Se llama a la funcion TabSwtich
            TabSwitch(4);
            //Se selecciona el objeto "btnIBack"
            btnIBack.Select();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(5);
            //Se selecciona el objeto "btnVBack"
            btnVBack.Select();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(6);
            //Se selecciona el objeto "btnDBack"
            btnDBack.Select();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(0);
            //Se selecciona el objeto "btnAddProduct"
            btnAddProduct.Select();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(0);
            //Se selecciona el objeto "btnDelProduct"
            btnDelProduct.Select();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(0);
            //Se selecciona el objeto "btnAddUnit"
            btnAddUnit.Select();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(0);
            //Se selecciona el objeto "btnProduct"
            btnProduct.Select();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(0);
            //Se selecciona el objeto "btnLogSales"
            btnLogSales.Select();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwtich
            TabSwitch(0);
            //Se selecciona el objeto "btnLogDevo"
            btnLogDevo.Select();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Estructura try
            try
            {
                if (!CerrarSesion)
                {
                    //Se cierra el objeto de clase "Form1" almacenado en el indice 0
                    //del arreglo "OpenForms" de la clase "Application"
                    Application.OpenForms[0].Close();
                }
            }
            catch{}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Se verifica si el valor que retorna la funcion "AgregarNuevoProducto" del objeto "bdd"
            //sea igual a true
            if (bdd.AgregarNuevoProducto(ref txtNomProducto, ref txtCodProducto, ref txtLabProducto, ref cboForma, ref txtPresentacion, cboTipoMedicamento, ref numPrecioCompra, ref numPrecioVenta, ref numUnidadesIniciales, ref cantProductos))
            {
                //Se llama a la funcion "LimpiarCampos"
                LimpiarCampos();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion "borrarProducto" del objeto "bdd"
            bdd.borrarProducto(ref cboProducto, this);
        }

        private void picLogDevo_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion "agregarUnidades" del objeto "bdd"
            bdd.agregarUnidades(ref cmbProducto, ref numUnidades, this);
        }

        private void btnCloseSession_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion "CS"
            CS();
        }
    }
}