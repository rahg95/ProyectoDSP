using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Avance_de_Proyecto_1
{
    public partial class Form3 : Form
    {
        //Se crea un objeto de clase "BDD"
        BDD bdd = new BDD();
        //Lista de Clase "TabPage"
        TabPage[] Paginas = new TabPage[7];
        //Se crea un objeto de clase "Form1"
        Form1 f1;
        //Variable que indica si se va a cerrar el form por cerrar sesion o por la X
        public bool CerrarSesion = false;
        //Variable que lleva el control del precio actual de la compra
        double costo = 0.00;

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

        public Form3(Form1 f1)
        {
            InitializeComponent();
            //Se crea una copia del parametro "f1" y se almacena en el objeto de la clase "f1"
            this.f1 = f1;
        }

        //Función que permite cambiar de una pestaña a otra
        private void TabChange(int origen, int destino)
        {
            //Se deshabilita el contenido de la pestaña actual
            tabContenedor.TabPages[origen].Enabled = false;
            //Se habilita el contenido de la pestaña de destino
            tabContenedor.TabPages[destino].Enabled = true;
            //Cambiamos de pestaña hacia la pestaña de destino
            tabContenedor.SelectedIndex = destino;
        }//Fin TabChange()

        //Funcion que se encarga de actualizar los elementos de un objeto ListBox
        private void ActualizarLista(ref ListBox lista)
        {
            //Se limpia el contenido del objeto "lista"
            lista.Items.Clear();
            //Objeto de clase "List<string>"
            List<string> items = new List<string>();
            //Se almacena el valor que retorna la funcion "ListaNombres" del objeto "bdd" en el objeto
            //"items"
            items = bdd.ListaNombres();
            //Variable que almacena la cantidad de elementos contenidos en el objeto "lista"
            int objetos = items.Count;
            //Estructura repetitiva que se ejecuta una cantidad equivalente a la cantidad
            //de elementos del objeto "lista"
            for (int i = 0; i < objetos; i++)
            {
                //Se agrega el contenido del elemento en el indice "i" del objeto "items" dentro
                //del objeto "lista"
                lista.Items.Add(items[i]);
            }

        }

        //Funcion que se encarga de actualizar los elementos de un objeto ListBox en base a un objeto
        //"Resultados"
        private void ActualizarLista(ref ListBox lista, List<string> Resultados)
        {
            //Variable que almacena la cantidad de elementos que contiene el objeto "Resultados"
            int cant = Resultados.Count;
            //Se limpia el contenido del objeto "lista"
            lista.Items.Clear();
            //Estructura repetitiva for
            for (int i = 0; i < cant; i++)
            {
                //Se añade el contenido del objeto "Resultados" en el indice "i"
                //al objeto "lista"
                lista.Items.Add(Resultados[i]);
            }
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

        //Funcion que se encarga realizar una busqueda dentro de los medicamentos en inventario
        private void Busqueda()
        {
            //Se verifica si el objeto "txtBusqueda" no contiene texto en su propiedad "Text"
            if (txtBusqueda.Text == "")
            {
                //Se llama a la funcion "ActualizarLista"
                ActualizarLista(ref lstItems);
            }
            else
            {
                //Se llama a la funcion "ActualizarLista"
                ActualizarLista(ref lstItems, bdd.ResultadosDeBusqueda(txtBusqueda.Text));
            }
            //Se verifica si la cantidad de items que contiene el objeto "lstItems" es igual a 0
            if (lstItems.Items.Count == 0)
            {
                //Se limpia el contenido del objeto "lstItems"
                lstItems.Items.Clear();
                //Se añaden un elemento al objeto "lstItems"
                lstItems.Items.Add("No se encontró ningún medicamento :( ...");
            }
        }

        //Funcion que se encarga de agregar un producto a la seccion de compra
        private void AgregarProductoVenta()
        {
            //Se evalua si el indice seleccionado del objeto "lstItems" es mayor a -1
            if (lstItems.SelectedIndex > -1 && lstItems.Text != "No se encontró ningún medicamento :( ...")
            {
                //Se agrega el texto del elemento seleccionado del objeto "lstItems" al objeto
                //"lstCompra"
                lstCompra.Items.Add(lstItems.SelectedItem);
                //Variable "double" el cual contiene el precio del producto seleccionado en el
                //objeto "lstItems"
                double precioActual = bdd.ActualizarCosto(ref lstCompra, ref lstItems);
                //Se reasigna el valor de la variable "costo"
                costo = costo + precioActual;
                //Se cambia el contenido de la propiedad "Text" del objeto "lblTotal"
                //para mostrar cual es el costo de la compra actual
                lblTotal.Text = "$" + costo;
            }
        }

        //Funcion que se encarga de devolver un producto de la lista de comprados
        private void DevolverProducto()
        {
            //Se verifica si el indice seleccionado del objeto "lstCompra" es mayor a -1
            if (lstCompra.SelectedIndex > -1)
            {
                //Se llama a la funcion "ActualizarCosto" del objeto "bdd" y su valor es almacenado
                //en una variable "double"
                double resta = bdd.ActualizarCosto(ref lstCompra);
                //Se remueve el elemento seleccionado en el objeto "lstCompra"
                lstCompra.Items.RemoveAt(lstCompra.SelectedIndex);
                //Se decrementa el valor de la variable "costo"
                costo = costo - resta;
                //Se cambia el contenido de la propiedad "Text" del objeto "lblTotal"
                //para mostrar cual es el costo de la compra actual
                lblTotal.Text = "$" + costo;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            //Se llama a la funcion "AlmacenarPaginas"
            AlmacenarPaginas(ref Paginas);
            //Se limpia el contenido de la coleccion "TabPages" del objeto "TabContenedor"
            tabContenedor.TabPages.Clear();
            //Se llama a la funcion TabSwtich
            TabSwitch(0);
            //Se selecciona el objeto "btnAddSale"
            btnAddSale.Select();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwitch
            TabSwitch(0);
            //Se selecciona el objeto "btnAddSale";
            btnAddSale.Select();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwitch
            TabSwitch(0);
            //Se selecciona el objeto "btnInventory"
            btnInventary.Select();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwitch
            TabSwitch(0);
            //Se selecciona el objeto "btnSoliDevo"
            btnSoliDevo.Select();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion "ActualizarLista"
            ActualizarLista(ref lstItems);
            //Se llama a la funcion TabSwitch
            TabSwitch(1);
            //Se selecciona el objeto "txtBusqueda"
            txtBusqueda.Select();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwitch
            TabSwitch(2);
            //Se selecciona el objeto "btnIAtras"
            btnIAtras.Select();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion TabSwitch
            TabSwitch(3);
            //Se selecciona el objeto "btnDevolucion"
            btnDevolucion.Select();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
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
            catch { }
        }

        private void btnCloseSession_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion "CS"
            CS();
        }

        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Se crea un objeto "Regex"
            Regex patron = new Regex(@"[^a-zA-Z0-9\s\b\-]$");
            //Se crea un objeto "Match" el cual almacena el resultado de una busqueda de la tecla que presiona el usuario
            //en cuanto a una expresion regular
            Match match = patron.Match(e.KeyChar.ToString());
            //Se evalua si la busqueda anterior tuvo exito
            if (match.Success)
            {
                //Se indica que el evento KeyPress fue controlado
                e.Handled = true;
            }
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            //Se llama a la funcion "Busqueda"
            Busqueda();
        }

        private void btnVentaProducto_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion AgregarProductoVenta
            AgregarProductoVenta();
        }

        private void btnFinalizarVenta_Click(object sender, EventArgs e)
        {
            //Se verifica si la funcion "EfectuarVenta" del objeto "bdd" retorna true
            if (bdd.EfectuarVenta(lstCompra))
            {
                //Se limpia el contenido del objeto "lstCompra"
                lstCompra.Items.Clear();
                //Se limpia el contenido del objeto "txtBusqueda"
                txtBusqueda.Text = "";
                //Se asigna un valor por defecto a la propiedad "Text" del objeto "lblTotal"
                lblTotal.Text = "$0.00";
            }
        }

        private void btnDevolver_Click(object sender, EventArgs e)
        {
            //Se llama a la funcion "DevolverProducto"
            DevolverProducto();
        }
    }
}