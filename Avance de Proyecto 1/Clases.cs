using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Data;

namespace Avance_de_Proyecto_1
{
    class BDD
    {
        //Atributos
        private SqlConnection bdd;
        private string DataBaseQuery;

        //Métodos
        public BDD()
        {
            //Se asigna un valor al atributo "DataBaseQuery"
            //DataBaseQuery = "Data Source=DESKTOP-J9IGSDS\\SQLEXPRESS;Initial Catalog=FarmaCOIN;Integrated Security=True";
            DataBaseQuery = "Data Source=(local);Initial Catalog=FarmaCOIN;Integrated Security=True";
        }

        //Funcion que se encarga de abrir la conexion con la base de datos
        public bool Conectar()
        {
            //Estructura try
            try
            {
                //Se instancia el objeto "bdd"
                bdd = new SqlConnection(DataBaseQuery);
                //Se abre la conexion con la base de datos
                bdd.Open();
                //Se retorna true
                return true;
            }
            //Estructura catch
            catch
            {
                //Se retorna false
                return false;
            }
        }
        //Metodo que nos devuelve conexion
        public SqlConnection Conexion()
        {
            //Creamos un objeto del tipo sqlConnection y dentro del objeto en su constructor llamamos la cadena de conexion
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["bdd"].ConnectionString);
            if (cn.State == ConnectionState.Open)//Validacion si hay una conexion
            {
                //Si hay una conexion la cerramos
                cn.Close();
            }
            else
            {
                //Si no hay una conexion la abrimos
                cn.Open();
            }
            //Retornamos el objeto
            return cn;
        }

        //Funcion que se encarga de cerrar la conexion con la base de datos
        private void Desconectar()
        {
            //Se cierra la conexion con la base de datos
            bdd.Close();
        }

        //Funcion que se encarga de realizar los procesos necesarios de inicios de sesion
        public string IniciarSesion(string user = "", string pass = "")
        {
            //Se llama a la funcion "Conectar"
            Conectar();
            //Se crea un objeto de tipo "SqlCommand"
            SqlCommand comando = new SqlCommand();
            //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
            comando.CommandText = "SELECT * FROM Usuarios WHERE Usuario = '" + user + "' AND Pass = '" + pass + "'";
            //Se asigna al objeto "comando" la base de datos en la cual
            //ejecutará las consultas indicadas previamente
            comando.Connection = bdd;
            //Se crea un objeto de la clase "SqlDataReader"
            SqlDataReader lector;
            //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
            //del objeto "comando" y su valor es almacenado en el objeto "lector"
            lector = comando.ExecuteReader();
            //Se evalua si el objeto "lector" tiene filas de resultados en su interior
            if (lector.HasRows)
            {
                //Se selecciona la siguiente fila de los resultado contenidos
                //dentro del objeto "lector"
                lector.Read();
                //Se retorna el valor contenido en la columna "TipoUsuario" de la fila
                //contenida dentro del objeto "lector"
                return lector["TipoUsuario"].ToString();
            }

            //Se llama a la funcion "Desconectar"
            Desconectar();
            //Se retorna "noMatch"
            return "noMatch";
        }

        //Funcion que se encarga de validar todos los aspectos necesarios para determinar
        //si se puede agregar un producto o no al inventario
        public bool AgregarNuevoProducto(ref TextBox txtNomProducto, ref TextBox txtCodProducto, ref TextBox txtLabProducto, ref ComboBox cboFormaProducto, ref TextBox txtPresentacion, ComboBox cboTipoMedicamento, ref NumericUpDown numPrecioCompra, ref NumericUpDown numPrecioVenta, ref NumericUpDown numUnidadesIniciales, ref int productos)
        {
            //Se llama a la funcion Conectar
            Conectar();
            //Se crea un objeto de clase "SqlCommand"
            SqlCommand comando = new SqlCommand();
            //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
            comando.CommandText = "SELECT * FROM Productos";
            //Se asigna al objeto "comando" la base de datos en la cual
            //ejecutará las consultas indicadas previamente
            comando.Connection = bdd;
            //Se verifica que el contenido de la propiedad "Text" del objeto "txtNomProducto"
            //sea diferente a ""
            if (txtNomProducto.Text != "")
            {
                //Se verifica que el contenido de la propiedad "Text" del objeto "txtCodProducto"
                //sea diferente a ""
                if (txtCodProducto.Text != "")
                {
                    //Variable que lleva la cuenta de la cantidad de coincidencias de productos con el mismo
                    //codigo identificador
                    int coincidencias = 0;
                    //Se crea un objeto de clase "SqlDataReader"
                    SqlDataReader lector;
                    //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
                    //del objeto "comando" y su valor es almacenado en el objeto "lector"
                    lector = comando.ExecuteReader();
                    //Se recorre cada una de las filas contenidas dentro del objeto "lector"
                    while (lector.Read())
                    {
                        //Se verifica si el contenido de la columna "CodigoProducto" en el objeto "lector"
                        //en la fila actual, es igual al valor de la propiedad "Text" del objeto
                        //"txtCodProducto"
                        if (lector["CodigoProducto"].ToString() == txtCodProducto.Text)
                        {
                            //Se aumenta en 1 el valor de la variable "coincidencias"
                            coincidencias++;
                        }
                    }
                    //Se verifica que el valor de la variable "coincidencias" sea igual a 0
                    if (coincidencias == 0)
                    {
                        //Se verifica que el contenido de la propiedad "Text" del objeto "txtLabProducto"
                        //sea diferente a ""
                        if (txtLabProducto.Text != "")
                        {
                            //Se verifica que el contenido de la propiedad "Text" del objeto "txtFormaProducto"
                            //sea diferente a ""
                            if (cboFormaProducto.Text != "")
                            {
                                //Se verifica que el contenido de la propiedad "Text" del objeto "txtPresentacion"
                                //sea diferente a ""
                                if (txtPresentacion.Text != "")
                                {
                                    //Se verifica que el contenido de la propiedad "Value" del objeto "numPrecioCompra"
                                    //sea mayor a 0
                                    if (numPrecioCompra.Value > 0)
                                    {
                                        //Se verifica que el contenido de la propiedad "Value" del objeto "numPrecioVenta"
                                        //sea mayor a 0
                                        if (numPrecioVenta.Value > 0)
                                        {
                                            //Se verifica que el contenido de la propiedad "Value" del objeto "numUnidadesIniciales"
                                            //sea mayor a 0
                                            if (numUnidadesIniciales.Value > 0)
                                            {
                                                //Se cierra el objeto "lector"
                                                lector.Close();
                                                //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
                                                //del objeto "comando" y su valor es almacenado en el objeto "lector"
                                                lector = comando.ExecuteReader();
                                                //Variable que almacena la cantidad de registros almacenados en la tabla "Productos"
                                                //de la base de datos
                                                int cantProductos = 0;
                                                //Variable que almacena el mayor de los indices de los productos
                                                int indice = 0;
                                                //Estructura repetitiva que se encarga de recorrer cada uno de las filas que estan
                                                //almacenadas en el objeto "lector"
                                                while (lector.Read())
                                                {
                                                    //Se aumenta en 1 la el valor de la variable "cantProductos"
                                                    cantProductos++;
                                                    //Se evalua si el valor de la variable "cantProductos" es igual a 1
                                                    if (cantProductos == 1)
                                                    {
                                                        //Se convierte a entero el contenido del objeto "lector" en el indice "id_producto"
                                                        //y es almacenado en la variable "indice"
                                                        indice = Convert.ToInt32(lector["id_producto"]);
                                                    }
                                                    else
                                                    {
                                                        //Se verifica si el valor en el objeto "lector" en el indice "id_producto"
                                                        //es mayor al valor de la variable "indice"
                                                        if (Convert.ToInt32(lector["id_producto"]) > indice)
                                                        {
                                                            //Se asigna a la variable "indice" el valor del objeto "lector" en el indice
                                                            //"id_producto" convertido a entero
                                                            indice = Convert.ToInt32(lector["id_producto"]);
                                                        }
                                                    }
                                                }
                                                //Se cierra el objeto "lector"
                                                lector.Close();
                                                //Se verifica si el valor de la variable "cantProductos" es mayor a 0
                                                if (cantProductos > 0)
                                                {
                                                    //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
                                                    comando.CommandText = "INSERT INTO Productos VALUES (" + (indice + 1) + ", '" + txtNomProducto.Text + "', '" + txtCodProducto.Text + "', '" + txtLabProducto.Text + "', '" + cboFormaProducto.Text + "', '" + txtPresentacion.Text + "', '" + cboTipoMedicamento.Text + "', " + numPrecioCompra.Value + ", " + numPrecioVenta.Value + ", " + numUnidadesIniciales.Value + ")";
                                                }else if (cantProductos == 0)
                                                {
                                                    //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
                                                    comando.CommandText = "INSERT INTO Productos VALUES (1, '" + txtNomProducto.Text + "', '" + txtCodProducto.Text + "', '" + txtLabProducto.Text + "', '" + cboFormaProducto.Text + "', '" + txtPresentacion.Text + "', '" + cboTipoMedicamento.Text + "', " + numPrecioCompra.Value + ", " + numPrecioVenta.Value + ", " + numUnidadesIniciales.Value + ")";
                                                }
                                                //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
                                                //del objeto "comando" y su valor es almacenado en el objeto "lector"
                                                lector = comando.ExecuteReader();
                                                //Se muestra un mensaje al usuario
                                                MessageBox.Show("El producto ha sido agregado al inventario exitosamente. :D", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                //Se retorna true
                                                return true;
                                            }
                                            else
                                            {
                                                //Se muestra un mensaje de error al usuario
                                                MessageBox.Show("Ingrese la cantidad de unidades iniciales que tendrá el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                //Se selecciona el objeto "numUnidadesIniciales"
                                                numUnidadesIniciales.Select();
                                            }
                                        }
                                        else
                                        {
                                            //Se muestra un mensaje de error al usuario
                                            MessageBox.Show("Ingrese el precio de venta del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            //Se selecciona el objeto "numPrecioVenta"
                                            numPrecioVenta.Select();
                                        }
                                    }
                                    else
                                    {
                                        //Se muestra un mensaje de error al usuario
                                        MessageBox.Show("Ingrese el precio de compra del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        //Se selecciona el objeto "numPrecioCompra"
                                        numPrecioCompra.Select();
                                    }
                                }
                                else
                                {
                                    //Se muestra un mensaje de error al usuario
                                    MessageBox.Show("Ingrese la presencacion en la que viene el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    //Se selecciona el objeto "txtPresentacion"
                                    txtPresentacion.Select();
                                }
                            }
                            else
                            {
                                //Se muestra un mensaje de error al usuario
                                MessageBox.Show("Ingrese la forma que tiene medicamento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //Se selecciona el objeto "txtFormaProducto"
                                cboFormaProducto.Select();
                            }
                        }
                        else
                        {
                            //Se muestra un mensaje de error al usuario
                            MessageBox.Show("Ingrese el nombre del laboratorio del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //Se selecciona el objeto "txtLabProducto"
                            txtLabProducto.Select();
                        }
                    }
                    else
                    {
                        //Se muestra un mensaje de error al usuario
                        MessageBox.Show("El codigo de producto ingresado ya está registrado. Por favor ingrese un codigo de producto distinto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //Se selecciona el objeto "txtCodProducto"
                        txtCodProducto.Select();
                    }
                }
                else
                {
                    //Se muestra un mensaje de error al usuario
                    MessageBox.Show("Ingrese el codigo del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //Se selecciona el objeto "txtCodProducto"
                    txtCodProducto.Select();
                }
            }
            else
            {
                //Se muestra un mensaje de error al usuario
                MessageBox.Show("Ingrese el nombre del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //Se selecciona el objeto "txtNomProducto"
                txtNomProducto.Select();
            }
            //Se llama a la funcion Desconectar
            Desconectar();
            //Se retorna false
            return false;
        }

        public bool mostrarProducto(ref ComboBox cboProducto)
        {
            //Se llama al metodo conectar
            Conectar();
            //Se crea un objeto de clase "SqlCommand"
            SqlCommand comando = new SqlCommand();
            //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
            comando.CommandText = "SELECT NombreProducto FROM Productos";
            //Se asigna al objeto "comando" la base de datos en la cual
            //ejecutará las consultas indicadas previamente
            comando.Connection = bdd;
            //Se crea un objeto de clase "SqlDataReader"
            SqlDataReader lector;
            //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
            //del objeto "comando" y su valor es almacenado en el objeto "lector"
            lector = comando.ExecuteReader();
            //Lipiamos el comboBox
            cboProducto.Items.Clear();
            if (lector.HasRows)
            {
                //S recorre la consulta
                while (lector.Read())
                {
                    //se agregan los valores de la consulta al cmbBox
                    cboProducto.Items.Add(lector["NombreProducto"]).ToString();
                }
                //Se selecciona el primer indice del objeto "cboProducto"
                cboProducto.SelectedIndex = 0;
                //Se llama a la funcion Desconectar
                Desconectar();
                //Se retorna "true"
                return true;
            }
            else
            {
                //Se muestra un mensaje de error
                MessageBox.Show("No hay productos en inventario para eliminar. Por favor, ingrese un nuevo producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //Se llama a la funcion Desconectar
            Desconectar();
            return false;
        }
        public void borrarProducto(ref ComboBox cboProducto, Form2 f2)
        {
            //Se llama al metodo conectar
            Conectar();
            //Se crea un objeto de clase "SqlCommand"
            SqlCommand comando = new SqlCommand();
            //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
            comando.CommandText = "DELETE FROM Productos WHERE  NombreProducto = '" + cboProducto.Text + "'";
            //Se asigna al objeto "comando" la base de datos en la cual
            //ejecutará las consultas indicadas previamente
            comando.Connection = bdd;
            //Se crea un objeto de clase "SqlDataReader"
            SqlDataReader lector;
            //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
            //del objeto "comando" y su valor es almacenado en el objeto "lector"
            //lector = comando.ExecuteReader();
            if (cboProducto.Text == "")
            {
                MessageBox.Show("No hay productos en inventario para eliminar. Por favor, ingrese un nuevo producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                f2.TabSwitch(0);
            }
            else
            {
                DialogResult respuesta;
                respuesta = MessageBox.Show("¿Esta seguro que desea eliminar permanentemente este producto?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
                    //del objeto "comando" y su valor es almacenado en el objeto "lector"
                    lector = comando.ExecuteReader();
                    MessageBox.Show("El producto " + cboProducto.Text + " Ha sido Eliminado", "infomacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Refrescamos el cbo
                    mostrarProducto(ref cboProducto);
                }

            }
        }

        public List<string> ListaNombres()
        {
            //Se llama a la funcion "Conectar"
            Conectar();
            //Se crea un objeto de tipo "SqlCommand"
            SqlCommand comando = new SqlCommand();
            //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
            comando.CommandText = "SELECT CodigoProducto, NombreProducto, PrecioVenta FROM Productos";
            //Se asigna al objeto "comando" la base de datos en la cual
            //ejecutará las consultas indicadas previamente
            comando.Connection = bdd;
            //Se crea un objeto de la clase "SqlDataReader"
            SqlDataReader lector;
            //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
            //del objeto "comando" y su valor es almacenado en el objeto "lector"
            lector = comando.ExecuteReader();
            //Se crea un objeto de clase "List<String>"
            List<string> items = new List<string>();
            //Se recorre cada uno de las filas de resultados almacenadas en el objeto "lector"
            while (lector.Read())
            {
                //Se almacena cada uno de los valores contenidos dentro de "lector" en el objeto "items"
                items.Add(lector[0] + " - " + lector[1] + " - $" + lector[2]);
            }

            //Se llama al metodo "Desconectar"
            Desconectar();
            
            //Se retorna el objeto "items"
            return items;
        }

        public void agregarUnidades(ref ComboBox cmbProducto, ref NumericUpDown numUnidades, Form2 f2)
        {
            //Se llama a la funcion "Conectar"
            Conectar();
            //Se crea un objeto de tipo "SqlCommand"
            SqlCommand comando = new SqlCommand();
            //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
            comando.CommandText = "UPDATE Productos SET Unidades = Unidades +'" + numUnidades.Value + "' WHERE NombreProducto = '" + cmbProducto.Text + "'";
            //Se asigna al objeto "comando" la base de datos en la cual
            //ejecutará las consultas indicadas previamente
            comando.Connection = bdd;
            //Se crea un objeto de la clase "SqlDataReader"
            SqlDataReader lector;
            //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
            //del objeto "comando" y su valor es almacenado en el objeto "lector"
            if (cmbProducto.Text == "")
            {
                MessageBox.Show("No hay productos en inventario para eliminar. Por favor, ingrese un nuevo producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                f2.TabSwitch(0);
            }
            else
            {
                DialogResult respuesta;
                respuesta = MessageBox.Show("¿Esta seguro que desea ingresar las unidades a este producto?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
                    //del objeto "comando" y su valor es almacenado en el objeto "lector"
                    lector = comando.ExecuteReader();
                    MessageBox.Show("Las unidades del producto " + cmbProducto.Text + " Han sido agregadas exitosamente!", "infomacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Refrescamos el cbo
                    mostrarProducto(ref cmbProducto);
                }

            }
        }

        //Metodo para mostrar datos de inventario
        public DataTable mostrarInventario()
        {
            //Creamos el objetio de tipo sqlDataAdapter
            //En su metodo constructor llamamos a la conexion y el procedimiento almacenado
            SqlDataAdapter da = new SqlDataAdapter("sp_mostrarInventario", Conexion());
            //Indicamos que el objeto es un procedimiento almacenado
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //Creamos el objeto del tipo DataTable
            DataTable dt = new DataTable();
            //Transferimos mediante el metodo fill los datos recogido al dataTable
            da.Fill(dt);
            //Retornamos el objeto
            return dt;
        }

        //Metodo para mostrar datos de inventario
        public DataTable mostrarDevoluciones()
        {
            //Creamos el objetio de tipo sqlDataAdapter
            //En su metodo constructor llamamos a la conexion y el procedimiento almacenado
            SqlDataAdapter da = new SqlDataAdapter("sp_mostrarDevoluciones", Conexion());
            //Indicamos que el objeto es un procedimiento almacenado
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //Creamos el objeto del tipo DataTable
            DataTable dt = new DataTable();
            //Transferimos mediante el metodo fill los datos recogido al dataTable
            da.Fill(dt);
            //Retornamos el objeto
            return dt;
        }

        public void Pedir_Devolucion(ref ComboBox cmbProducto, ref NumericUpDown numUnidades)
        {//Se llama a la funcion conectar
            Conectar();
            //se crea un objeto de tipo "SqlCommand"
            SqlCommand comando = new SqlCommand();
            //se agrega contenido a la propiedad del objeto sqlCommand
            comando.CommandText = "UPDATE Productos SET Unidades = Unidades +" + numUnidades.Value + "' WHERE NombreProducto ='" + cmbProducto.Text + "'";
            //se asigna al objeto "comando la bdd den la cual ejecutara las consultas"
            comando.Connection = bdd;
            //se crea un objeto de la clase sqldatareader
            SqlDataReader lector;
            //se ejecuta la consulta dentro de la propiedad commandText y su valor es almacenado en el objeto lector
            lector = comando.ExecuteReader();

        }

        public List<string> ResultadosDeBusqueda(string busqueda)
        {
            //Se crea un objeto de clase "List<string>"
            List<string> resultados = new List<string>();
            //Se crea un objeto de clase "SqlCommand"
            //Se llama a la funcion "Conectar"
            Conectar();
            SqlCommand command = new SqlCommand();
            //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
            command.CommandText = "SELECT CodigoProducto, NombreProducto, PrecioVenta FROM Productos WHERE CodigoProducto LIKE '%" + busqueda +"%'";
            //Se asigna al objeto "comando" la base de datos en la cual
            //ejecutará las consultas indicadas previamente
            command.Connection = bdd;
            //Se crea un objeto de clase "SqlDataReader"
            SqlDataReader lector;
            //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
            //del objeto "command" y su valor es almacenado en el objeto "lector"
            lector = command.ExecuteReader();
            //Se recorre cada uno de las filas de resultados almacenadas en el objeto "lector"
            while (lector.Read())
            {
                //Se almacena cada uno de los valores contenidos dentro de "lector" en el objeto "resueltados"
                resultados.Add(lector[0] + " - " + lector[1] + " - $" + lector[2]);
            }
            //Se llamaa la funcio "Desconectar"
            Desconectar();
            //Se retorna el objeto "resultados"
            return resultados;
        }

        //Funcion que se encarga de Actualizar el costo de la actual compra de productos
        public double ActualizarCosto(ref ListBox Compras, ref ListBox Inventario)
        {
            //Se llama a la funcion "Conectar"
            Conectar();
            //Se crea una variable string que contendrá el Codigo de producto del elemento seleccionado
            //en el objeto "Inventario"
            string codigo = Inventario.Text;
            //Se reasigna el valor de la variable "codigo"
            codigo = codigo[0].ToString() + codigo[1].ToString() + codigo[2].ToString() + codigo[3].ToString();
            //Se crea un objeto de tipo "SqlCommand"
            SqlCommand comando = new SqlCommand();
            //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
            comando.CommandText = "SELECT CodigoProducto, NombreProducto, PrecioVenta FROM Productos WHERE CodigoProducto = '" + codigo + "'";
            //Se asigna al objeto "comando" la base de datos en la cual
            //ejecutará las consultas indicadas previamente
            comando.Connection = bdd;
            //Se crea un objeto de la clase "SqlDataReader"
            SqlDataReader lector;
            //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
            //del objeto "comando" y su valor es almacenado en el objeto "lector"
            lector = comando.ExecuteReader();
            //Se crea una variable la cual almacenará el precio del producto seleccionado
            double precio = 0;
            //Se evalua si el objeto "lector" tiene filas en su interior
            if (lector.HasRows)
            {
                //Se llama a la funcion "Read" del objeto "lector"
                lector.Read();
                //Se asigna el valor del objeto "lector" en el indic "PrecioVenta"
                //al objeto "precio" pero antes convertido a double
                precio = Convert.ToDouble(lector["PrecioVenta"]);
            }
            //Se llama a la funcion "Desconectar"
            Desconectar();
            //Se retorna el valor de la variable "precio"
            return precio;
        }

        //Funcion que se encarga de Actualizar el costo de la actual compra de productos
        public double ActualizarCosto(ref ListBox Compras)
        {
            //Se llama a la funcion "Conectar"
            Conectar();
            //Se crea una variable string que contendrá el Codigo de producto del elemento seleccionado
            //en el objeto "Inventario"
            string codigo = Compras.Text;
            //Se reasigna el valor de la variable "codigo"
            codigo = codigo[0].ToString() + codigo[1].ToString() + codigo[2].ToString() + codigo[3].ToString();
            //Se crea un objeto de tipo "SqlCommand"
            SqlCommand comando = new SqlCommand();
            //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
            comando.CommandText = "SELECT CodigoProducto, NombreProducto, PrecioVenta FROM Productos WHERE CodigoProducto = '" + codigo + "'";
            //Se asigna al objeto "comando" la base de datos en la cual
            //ejecutará las consultas indicadas previamente
            comando.Connection = bdd;
            //Se crea un objeto de la clase "SqlDataReader"
            SqlDataReader lector;
            //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
            //del objeto "comando" y su valor es almacenado en el objeto "lector"
            lector = comando.ExecuteReader();
            //Se crea una variable la cual almacenará el precio del producto seleccionado
            double precio = 0;
            //Se evalua si el objeto "lector" tiene filas en su interior
            if (lector.HasRows)
            {
                //Se llama a la funcion "Read" del objeto "lector"
                lector.Read();
                //Se asigna el valor del objeto "lector" en el indic "PrecioVenta"
                //al objeto "precio" pero antes convertido a double
                precio = Convert.ToDouble(lector["PrecioVenta"]);
            }
            //Se llama a la funcion "Desconectar"
            Desconectar();
            //Se retorna el valor de la variable "precio"
            return precio;
        }

        //Funcion que se encarga de Realizar una venta
        public bool EfectuarVenta(ListBox comprados)
        {
            //Se evalua si el objeto "comprados" tiene al menos al menos 1 elemento en su interior
            if (comprados.Items.Count >= 1)
            {
                //Variable que almacena la cantidad de elementos que posee el objeto "comprados"
                int cantProductos = comprados.Items.Count;
                //Arreglo de clase "string" que almacenará el codigo de cada uno de los productos
                //que se desean comprar (almacenados en el objeto "comprados")
                string[] productos = new string[cantProductos];
                //Estructura repetitiva for
                for (int i = 0; i < cantProductos; i++)
                {
                    //Se selecciona el elemento del objeto "comprados" en el indice "i"
                    comprados.SelectedIndex = i;
                    //Variable string que guarda la informacion del producto seleccionado dentro
                    //del objeto "comprados"
                    string info = comprados.Text;
                    //Se asigna un valor para el objeto "productos" en el indice "i"
                    productos[i] = info[0].ToString() + info[1].ToString() + info[2].ToString() + info[3].ToString();
                }
                //Se llama a la funcion "Conectar"
                Conectar();
                //Se crea un objeto de tipo "SqlCommand"
                SqlCommand comando = new SqlCommand();
                //Se asigna al objeto "comando" la base de datos en la cual
                //ejecutará las consultas indicadas previamente
                comando.Connection = bdd;
                //Se agrega contenido a la propiedad "CommandText" del objeto "SqlCommand"
                comando.CommandText = "SELECT * FROM Productos WHERE ";
                //Se crea un objeto de la clase "SqlDataReader"
                SqlDataReader lector;
                //Variable string
                string insertString = "INSERT INTO Ventas VALUES";
                //Estructura repetitiva for
                for (int j = 0; j < cantProductos; j++)
                {
                    //Se evalua si el valor de la variabe "j" igual al resultado de la resta de (cantProductos - 1)
                    if (j == (cantProductos - 1))
                    {
                        //Se agrega contenido a la propiedad "CommandText" del objeto "comando"
                        comando.CommandText = "SELECT * FROM Productos WHERE CodigoProducto = '" + productos[j] + "'";
                        //Se llama a la funcion "ExecuteReader" del objeto "comando" y su valor se almacena en el objeto "lector"
                        lector = comando.ExecuteReader();
                        //Se llama a la funcion "Read" del objeto "lector"
                        lector.Read();
                        //Se agrega contenido a la propiedad "CommandText" del objeto "comando"
                        insertString += "('" + lector["CodigoProducto"].ToString() + "', '" + lector["NombreProducto"].ToString() + "', " + Convert.ToDecimal(lector["PrecioVenta"]) + ", '" + (DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year) + "')";
                        //Se cierra el objeto "lector"
                        lector.Close();
                    }
                    else
                    {
                        //Se agrega contenido a la propiedad "CommandText" del objeto "comando"
                        comando.CommandText = "SELECT * FROM Productos WHERE CodigoProducto = '" + productos[j] + "'";
                        //Se llama a la funcion "ExecuteReader" del objeto "comando" y su valor se almacena en el objeto "lector"
                        lector = comando.ExecuteReader();
                        //Se llama a la funcion "Read" del objeto "lector"
                        lector.Read();
                        //Se agrega contenido a la propiedad "CommandText" del objeto "comando"
                        insertString += "('" + lector["CodigoProducto"].ToString() + "', '" + lector["NombreProducto"].ToString() + "', " + Convert.ToDecimal(lector["PrecioVenta"]) + ", '" + (DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year) + "'), ";
                        //Se cierra el objeto "lector"
                        lector.Close();
                    }
                }
                //Variable int contador
                int l = 0;
                //Se asigna un nuevo valor a la propiedad "CommandText" del objeto "comando"
                comando.CommandText = insertString;
                //Se ejecuta la consulta contenida dentro de la propiedad "CommandText"
                //del objeto "comando" y su valor es almacenado en el objeto "lector"
                lector = comando.ExecuteReader();
                //Se llama a la funcion "Desconectar"
                Desconectar();
                //Se Muestra un mensaje al usuario
                MessageBox.Show("La compra ha sido efectuada correctamente :D", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Se retorna "true"
                return true;
            }
            //Se retorna "false"
            return false;
        }
    }
}