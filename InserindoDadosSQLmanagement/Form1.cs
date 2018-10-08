using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Sql;
using System.Data.SqlClient;

using System.Windows;
using Opc.Da;
using System.Windows.Threading;
using System.Windows.Media;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        
        #region private fields

        //private bool motorActive;
        //private int motorSpeed;
        //private bool autManSwitch;

        private bool motorActive; //
        private int temperatura;
        private bool LSH_02;
        private bool LSL_01;
        private int Temperatura_TQ2;

        
        private int statusConexaoSilas = 0;
        private int var_modificada = 0;
        private string str_cmd = "";
        private int int_criar_tblUsuario = 0;
        private int int_criar_tblServidores = 0;
        private int int_criar_tblItens = 0;
        private string str_nome_tabela_valor="";
        private int int_id_usuario;



        DispatcherTimer tmr = new DispatcherTimer();


        #endregion


        #region OPC private fields

        private Server server;
        private OpcCom.Factory fact = new OpcCom.Factory();

        private Subscription groupRead;
        private SubscriptionState groupState;

        private Subscription groupWrite;
        private SubscriptionState groupStateWrite;

        private List<Item> itemsList = new List<Item>();

        #endregion


        public Form1()
        {
            InitializeComponent();


            dag_g5_lista_usuarios.AutoGenerateColumns = false;
            dag_g3_tabela_servidores.AutoGenerateColumns = false;
            dag_g2_preencher_tabela_itens.AutoGenerateColumns = false;
            dag_g1_preencher_tabela_servidores_conectados.AutoGenerateColumns = false;

            preencher_cmb_g2_tipo_de_valores_definidos();
            atualizar_cmb_g1_servidores();

            btn_g1_conectar_db.Focus();
            

            button1_Menu_Conexao.BackColor    = System.Drawing.Color.White;  //1-
            button1_Menu_Conexao.ForeColor    = System.Drawing.Color.Blue;   //1-
            button2_Menu_Itens.BackColor      = System.Drawing.Color.Blue;   //2
            button2_Menu_Itens.ForeColor      = System.Drawing.Color.White;  //2
            button3_Menu_Servidores.BackColor = System.Drawing.Color.Blue;   //3
            button3_Menu_Servidores.ForeColor = System.Drawing.Color.White;  //3
            button4_Menu_Database.BackColor   = System.Drawing.Color.Blue;   //4
            button4_Menu_Database.ForeColor   = System.Drawing.Color.White;  //4
            button5_Menu_Usuarios.BackColor   = System.Drawing.Color.Blue;   //5
            button5_Menu_Usuarios.ForeColor   = System.Drawing.Color.White;  //5
            button6_Menu_WebSite.BackColor    = System.Drawing.Color.Blue;   //6
            button6_Menu_WebSite.ForeColor    = System.Drawing.Color.White;  //6

            groupBox1_Conexao.Visible = true;
            groupBox2_Itens.Visible = false;
            groupBox3_Servidores.Visible = false;
            groupBox4_Database.Visible = false;
            groupBox5_Usuarios.Visible = false;
            groupBox6_Site.Visible = false;
        }

      

        private void button1_Menu_Conexao_Click(object sender, EventArgs e)
        {
            button1_Menu_Conexao.BackColor    = System.Drawing.Color.White;  //1-
            button1_Menu_Conexao.ForeColor    = System.Drawing.Color.Blue;   //1-
            button2_Menu_Itens.BackColor      = System.Drawing.Color.Blue;   //2
            button2_Menu_Itens.ForeColor      = System.Drawing.Color.White;  //2
            button3_Menu_Servidores.BackColor = System.Drawing.Color.Blue;   //3
            button3_Menu_Servidores.ForeColor = System.Drawing.Color.White;  //3
            button4_Menu_Database.BackColor   = System.Drawing.Color.Blue;   //4
            button4_Menu_Database.ForeColor   = System.Drawing.Color.White;  //4
            button5_Menu_Usuarios.BackColor   = System.Drawing.Color.Blue;   //5
            button5_Menu_Usuarios.ForeColor   = System.Drawing.Color.White;  //5
            button6_Menu_WebSite.BackColor    = System.Drawing.Color.Blue;   //6
            button6_Menu_WebSite.ForeColor    = System.Drawing.Color.White;  //6


            groupBox1_Conexao.Visible = true;
            groupBox2_Itens.Visible = false;
            groupBox3_Servidores.Visible = false;
            groupBox4_Database.Visible = false;
            groupBox5_Usuarios.Visible = false;
            groupBox6_Site.Visible = false;

            atualizar_cmb_g1_servidores();

        }

        private void button2_Menu_Itens_Click(object sender, EventArgs e)
        {
            button1_Menu_Conexao.BackColor    = System.Drawing.Color.Blue;   //1
            button1_Menu_Conexao.ForeColor    = System.Drawing.Color.White;  //1
            button2_Menu_Itens.BackColor      = System.Drawing.Color.White;  //2-
            button2_Menu_Itens.ForeColor      = System.Drawing.Color.Blue;   //2-
            button3_Menu_Servidores.BackColor = System.Drawing.Color.Blue;   //3
            button3_Menu_Servidores.ForeColor = System.Drawing.Color.White;  //3
            button4_Menu_Database.BackColor   = System.Drawing.Color.Blue;   //4
            button4_Menu_Database.ForeColor   = System.Drawing.Color.White;  //4
            button5_Menu_Usuarios.BackColor   = System.Drawing.Color.Blue;   //5
            button5_Menu_Usuarios.ForeColor   = System.Drawing.Color.White;  //5
            button6_Menu_WebSite.BackColor    = System.Drawing.Color.Blue;   //6
            button6_Menu_WebSite.ForeColor    = System.Drawing.Color.White;  //6


            groupBox1_Conexao.Visible = false;
            groupBox2_Itens.Visible = true;
            groupBox3_Servidores.Visible = false;
            groupBox4_Database.Visible = false;
            groupBox5_Usuarios.Visible = false;
            groupBox6_Site.Visible = false;

            atualizar_cmb_g3_login_e_senha();
            atualizar_cmb_g2_item_e_endereco();
        }

        private void button3_Menu_Servidores_Click(object sender, EventArgs e)
        {
            button1_Menu_Conexao.BackColor    = System.Drawing.Color.Blue;   //1
            button1_Menu_Conexao.ForeColor    = System.Drawing.Color.White;  //1
            button2_Menu_Itens.BackColor      = System.Drawing.Color.Blue;   //2
            button2_Menu_Itens.ForeColor      = System.Drawing.Color.White;  //2
            button3_Menu_Servidores.BackColor = System.Drawing.Color.White;  //3-
            button3_Menu_Servidores.ForeColor = System.Drawing.Color.Blue;   //3-
            button4_Menu_Database.BackColor   = System.Drawing.Color.Blue;   //4
            button4_Menu_Database.ForeColor   = System.Drawing.Color.White;  //4
            button5_Menu_Usuarios.BackColor   = System.Drawing.Color.Blue;   //5
            button5_Menu_Usuarios.ForeColor   = System.Drawing.Color.White;  //5
            button6_Menu_WebSite.BackColor    = System.Drawing.Color.Blue;   //6
            button6_Menu_WebSite.ForeColor    = System.Drawing.Color.White;  //6


            groupBox1_Conexao.Visible = false;
            groupBox2_Itens.Visible = false;
            groupBox3_Servidores.Visible = true;
            groupBox4_Database.Visible = false;
            groupBox5_Usuarios.Visible = false;
            groupBox6_Site.Visible = false;

            atualizar_cmb_g3_login_e_senha();
            atualizar_dag_g3_preencher_tabela_servidores();
            browser_servidor_OPC();
        }

        private void button4_Menu_Database_Click(object sender, EventArgs e)
        {
            button1_Menu_Conexao.BackColor    = System.Drawing.Color.Blue;   //1
            button1_Menu_Conexao.ForeColor    = System.Drawing.Color.White;  //1
            button2_Menu_Itens.BackColor      = System.Drawing.Color.Blue;   //2
            button2_Menu_Itens.ForeColor      = System.Drawing.Color.White;  //2
            button3_Menu_Servidores.BackColor = System.Drawing.Color.Blue;   //3
            button3_Menu_Servidores.ForeColor = System.Drawing.Color.White;  //3
            button4_Menu_Database.BackColor   = System.Drawing.Color.White;  //4-
            button4_Menu_Database.ForeColor   = System.Drawing.Color.Blue;   //4-
            button5_Menu_Usuarios.BackColor   = System.Drawing.Color.Blue;   //5
            button5_Menu_Usuarios.ForeColor   = System.Drawing.Color.White;  //5
            button6_Menu_WebSite.BackColor    = System.Drawing.Color.Blue;   //6
            button6_Menu_WebSite.ForeColor    = System.Drawing.Color.White;  //6


            groupBox1_Conexao.Visible = false;
            groupBox2_Itens.Visible = false;
            groupBox3_Servidores.Visible = false;
            groupBox4_Database.Visible = true;
            groupBox5_Usuarios.Visible = false;
            groupBox6_Site.Visible = false;
        }

        private void button5_Menu_Usuarios_Click(object sender, EventArgs e)
        {
            button1_Menu_Conexao.BackColor    = System.Drawing.Color.Blue;   //1
            button1_Menu_Conexao.ForeColor    = System.Drawing.Color.White;  //1
            button2_Menu_Itens.BackColor      = System.Drawing.Color.Blue;   //2
            button2_Menu_Itens.ForeColor      = System.Drawing.Color.White;  //2
            button3_Menu_Servidores.BackColor = System.Drawing.Color.Blue;   //3
            button3_Menu_Servidores.ForeColor = System.Drawing.Color.White;  //3
            button4_Menu_Database.BackColor   = System.Drawing.Color.Blue;   //4
            button4_Menu_Database.ForeColor   = System.Drawing.Color.White;  //4
            button5_Menu_Usuarios.BackColor   = System.Drawing.Color.White;  //5-
            button5_Menu_Usuarios.ForeColor   = System.Drawing.Color.Blue;   //5-
            button6_Menu_WebSite.BackColor    = System.Drawing.Color.Blue;   //6
            button6_Menu_WebSite.ForeColor    = System.Drawing.Color.White;  //6


            groupBox1_Conexao.Visible = false;
            groupBox2_Itens.Visible = false;
            groupBox3_Servidores.Visible = false;
            groupBox4_Database.Visible = false;
            groupBox5_Usuarios.Visible = true;
            groupBox6_Site.Visible = false;

            atualizar_cmb_g5_login_e_senha();
            atualizar_dag_g5_preencher_tabela_usuarios();
        }

        private void button6_Menu_WebSite_Click(object sender, EventArgs e)
        {

            button1_Menu_Conexao.BackColor    = System.Drawing.Color.Blue;   //1
            button1_Menu_Conexao.ForeColor    = System.Drawing.Color.White;  //1
            button2_Menu_Itens.BackColor      = System.Drawing.Color.Blue;   //2
            button2_Menu_Itens.ForeColor      = System.Drawing.Color.White;  //2
            button3_Menu_Servidores.BackColor = System.Drawing.Color.Blue;   //3
            button3_Menu_Servidores.ForeColor = System.Drawing.Color.White;  //3
            button4_Menu_Database.BackColor   = System.Drawing.Color.Blue;   //4
            button4_Menu_Database.ForeColor   = System.Drawing.Color.White;  //4
            button5_Menu_Usuarios.BackColor   = System.Drawing.Color.Blue;   //5
            button5_Menu_Usuarios.ForeColor   = System.Drawing.Color.White;  //5
            button6_Menu_WebSite.BackColor    = System.Drawing.Color.White;  //6-
            button6_Menu_WebSite.ForeColor    = System.Drawing.Color.Blue;   //6-


            groupBox1_Conexao.Visible = false;
            groupBox2_Itens.Visible = false;
            groupBox3_Servidores.Visible = false;
            groupBox4_Database.Visible = false;
            groupBox5_Usuarios.Visible = false;
            groupBox6_Site.Visible = true;
        }
        private void btn_g4_testar_conexao_Click(object sender, EventArgs e)
        {

            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
          //  "Integrated Security=SSPI;Data Source=SILAS-PC\\SQLEXPRESS;Persist Security Info=False;User ID=sa;Initial Catalog=dbPims"
            
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                txb_g4_status_conexao.Text = "Conexão Realizada com Sucesso!";
                con.Close();
           }
           catch(Exception)
           {
                txb_g4_status_conexao.Text = "Falhou Conexão";
           }
            

        }

        private void btn_g1_conectar_db_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                MessageBox.Show("A conexão com o Banco de Dados foi realizada com sucesso");
                con.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("A conexão com o Banco de Dados NÃO ESTÁ CONFIGURADA. Para configurá-la acesse o menu 'Configuração Banco de Dados'. ");
            }

            atualizar_cmb_g1_servidores();
        }

        private void btn_g5_adicionar_usuario_Click(object sender, EventArgs e)
        {
            if(txb_g5_login.Text != "")
            {
                
                if (txb_g5_senha.Text != "")
                {
                    string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                    SqlConnection con = new SqlConnection(connectionString);
                    try
                    {
                        con.Open();
                        txb_g5_status.Text = "Deu Certo!!!";

                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandText = "Insert into tblUsuarios([login] , [senha]) values('" + txb_g5_login.Text + "','" + txb_g5_senha.Text + "')";

                        try
                        {
                            cmd.ExecuteNonQuery();
                            txb_g5_status.Text = "Usuário Adicionado!";
                            MessageBox.Show("Usuário adicionado com sucesso!");
                        }
                        catch (Exception)
                        {
                            txb_g5_status.Text = "Não foi possível adicionar o usuário.";
                            MessageBox.Show("Esse Login JÁ EXISTE. Tente outro login!");
                        }
                    }
                    catch (Exception)
                    {
                        txb_g5_status.Text = "Não deu certo.";
                        MessageBox.Show("Erro SQL ao tentar adicionar o usuário.");
                    }

                    txb_g5_login.Text = "";
                    txb_g5_senha.Text = "";

                    atualizar_cmb_g5_login_e_senha();
                    atualizar_dag_g5_preencher_tabela_usuarios();  
                }
                else
                {
                    MessageBox.Show("Não é possível adicionar um usuário sem SENHA válida. Preencha esse campo!");
                }
            }
            else
            {
                MessageBox.Show("Não é possível adicionar um usuário sem LOGIN válido. Preencha esse campo!");
            }

            
        }

        

        private void btn_g5_remover_usuario_Click(object sender, EventArgs e)
        {
            if (cmb_g5_login_remover.Text != "")
            {
                string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                SqlConnection con = new SqlConnection(connectionString);
                try
                {
                    con.Open();
                    txb_g5_status.Text = "Deu Certo!!!";

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "Delete from tblUsuarios where login='" + cmb_g5_login_remover.Text + "'";

                    try
                    {
                        cmd.ExecuteNonQuery();
                        txb_g5_status.Text = "Usuário removido!";
                        MessageBox.Show("Usuário removido com sucesso!");
                    }
                    catch (Exception)
                    {
                        txb_g5_status.Text = "Não removeu o usuário.";
                        MessageBox.Show("Não foi possível remover o usuário!");
                    }
                }
                catch (Exception)
                {
                    txb_g5_status.Text = "Não deu certo.";
                    MessageBox.Show("Erro SQL ao tentar remover o usuário.");
                }

                cmb_g5_login_remover.Text = "";
                txb_g5_senha_remover_usuario.Text = "";

                atualizar_cmb_g5_login_e_senha();
                atualizar_dag_g5_preencher_tabela_usuarios();
            }
            else
            {
                MessageBox.Show("Não é possivel remover um usuário sem escolher o LOGIN do usuário a ser removido.");
            }
        }

        private void btn_g5_substituir_usuario_Click(object sender, EventArgs e)
        {
            if (txb_g5_login_substituir_usuario.Text != "")
            {
                if (txb_g5_senha_substituir_usuario.Text != "")
                {
                    if (cmb_g5_login_substituir_usuario.Text != "")
                    {

                        string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                        SqlConnection con = new SqlConnection(connectionString);
                        try
                        {
                            con.Open();
                            txb_g5_status.Text = "Deu Certo!!!";

                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandText = "Update tblUsuarios set login='" + txb_g5_login_substituir_usuario.Text + "',senha='" + txb_g5_senha_substituir_usuario.Text + "' where login='" + cmb_g5_login_substituir_usuario.Text + "' ;";

                            try
                            {
                                cmd.ExecuteNonQuery();
                                txb_g5_status.Text = "Usuário substituido!";
                                MessageBox.Show("Usuário substituido com sucesso!");
                            }
                            catch (Exception)
                            {
                                txb_g5_status.Text = "Não substituiu o Usuário.";
                                MessageBox.Show("Não foi possível substituir o Usuário.");
                            }

                        }
                        catch (Exception)
                        {
                            txb_g5_status.Text = "Não deu certo.";
                            MessageBox.Show("Erro SQL ao tentar substituir o Usuário.");
                        }

                        txb_g5_login_substituir_usuario.Text = "";
                        txb_g5_senha_substituir_usuario.Text = "";
                        cmb_g5_login_substituir_usuario.Text = "";
                        txb_g5_senha_a_ser_substituida.Text = "";

                        atualizar_cmb_g5_login_e_senha();
                        atualizar_dag_g5_preencher_tabela_usuarios();
                    }
                    else
                    {
                        MessageBox.Show("Não é possível substituir o Usuário sem escolher o LOGIN no qual será substituído.");
                    }
                }
                else
                {
                    MessageBox.Show("Não é possível substituir o Usuário sem SENHA válida. Preencha esse campo!");
                }
            }
            else
            {
                MessageBox.Show("Não é possível substituir o Usuário sem LOGIN válido. Preencha esse campo!");
            }

            
        }


        private void cmb_g5_login_remover_SelectedIndexChanged(object sender, EventArgs e)
        {

            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select senha from tblUsuarios where login='" + cmb_g5_login_remover.Text + "' ;", con);

            try
            {
                con.Open();
                cmd1.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd1;
                DataSet ds = new DataSet();
                da.Fill(ds, "senha");
                string senha_encontrada = "";

                senha_encontrada = ((string)cmd1.ExecuteScalar());
                txb_g5_senha_remover_usuario.Text = senha_encontrada;
                con.Close();

            }
            catch (Exception)
            {
            }
         
        }

        private void cmb_g5_login_substituir_usuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select senha from tblUsuarios where login='" + cmb_g5_login_substituir_usuario.Text + "' ;", con);

            try
            {
                con.Open();
                cmd1.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd1;
                DataSet ds = new DataSet();
                da.Fill(ds, "senha");
                string senha_encontrada = "";

                senha_encontrada = ((string)cmd1.ExecuteScalar());

                txb_g5_senha_a_ser_substituida.Text = senha_encontrada;
                con.Close();

            }
            catch (Exception)
            {
            }
            
        }


        private void btn_g3_adiciona_servidor_Click(object sender, EventArgs e)
        {
            
                //if (txb_g3_servidor_adicionar.Text != "")
                if (txb_g3_servidor_adicionar.Text != "")
                {
                    string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                    SqlConnection con = new SqlConnection(connectionString);
                    try
                    {
                        con.Open();
                        txb_g3_status.Text = "Deu Certo!!!";
                        
                        SqlCommand cmd = con.CreateCommand();
                        //cmd.CommandText = "Insert into tblServidores([nome_servidor]) values('" + txb_g3_servidor_adicionar.Text + "')";
                          cmd.CommandText = "Insert into tblServidores([nome_servidor]) values('" + txb_g3_servidor_adicionar.Text + "')";

                        try
                        {
                            cmd.ExecuteNonQuery();
                            txb_g3_status.Text = "Servidor Adicionado!";
                            MessageBox.Show("Servidor adicionado com sucesso!");
                        }
                        catch (Exception)
                        {
                            txb_g3_status.Text = "Não foi possível adicionar o Servidor.";
                            MessageBox.Show("Não foi possível adicionar o Servidor.");
                        }
                    }
                    catch (Exception)
                    {
                        txb_g3_status.Text = "Não deu certo.";
                        MessageBox.Show("Erro SQL ao tentar adicionar o Servidor.");
                    }

                    txb_g3_servidor_adicionar.Text = "";

                    atualizar_cmb_g3_login_e_senha();
                    atualizar_dag_g3_preencher_tabela_servidores();
                    
                }
                else
                {
                    MessageBox.Show("Não é possível adicionar um usuário sem nome de Servidor válido. Preencha esse campo!");
                }
            
           
        }


        private void btn_g3_remover_servidor_Click(object sender, EventArgs e)
        {
            if (cmb_g3_servidores_remover.Text != "")
            {
                string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                SqlConnection con = new SqlConnection(connectionString);
                try
                {
                    con.Open();
                    txb_g3_status.Text = "Deu Certo!!!";

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "Delete from tblServidores where nome_servidor='" + cmb_g3_servidores_remover.Text + "'"; 

                    try
                    {
                        cmd.ExecuteNonQuery();
                        txb_g3_status.Text = "Servidor removido!";
                        MessageBox.Show("Servidor removido com sucesso!");
                    }
                    catch (Exception)
                    {
                        txb_g3_status.Text = "Não removeu o Servidor.";
                        MessageBox.Show("Não foi possível remover o Servidor!");
                    }
                }
                catch (Exception)
                {
                    txb_g3_status.Text = "Não deu certo.";
                    MessageBox.Show("Erro SQL ao tentar remover o Servidor.");
                }

                cmb_g3_servidores_remover.Text = "";

                atualizar_cmb_g3_login_e_senha();
                atualizar_dag_g3_preencher_tabela_servidores();
            }
            else
            {
                MessageBox.Show("Não é possivel remover um usuário sem escolher o nome do Servidor a ser removido. Escolha uma opção!");
            }
        }


        private void btn_g3_substituir_servidor_Click(object sender, EventArgs e)
        {
            if (cmb_g3_substituir_servidor.Text != "")
            {
                if (txb_g3_substituir_servidor.Text != "")
                {

                    string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                    SqlConnection con = new SqlConnection(connectionString);
                    try
                    {
                        con.Open();
                        txb_g3_status.Text = "Deu Certo!!!";

                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandText = "Update tblServidores set nome_servidor='" + txb_g3_substituir_servidor.Text + "' where nome_servidor='" + cmb_g3_substituir_servidor.Text + "' ;";

                        try
                        {
                            cmd.ExecuteNonQuery();
                            txb_g3_status.Text = "Servidor substituído!";
                            MessageBox.Show("Servidor substituido com sucesso!");
                        }
                        catch (Exception)
                        {
                            txb_g3_status.Text = "Não substituiu o Servidor.";
                            MessageBox.Show("Não foi possível substituir o Servidor.");
                        }

                    }
                    catch (Exception)
                    {
                        txb_g3_status.Text = "Não deu certo.";
                        MessageBox.Show("Erro SQL ao tentar substituir o Servidor.");
                    }

                    cmb_g3_substituir_servidor.Text = "";
                    txb_g3_substituir_servidor.Text = "";

                    atualizar_cmb_g3_login_e_senha();
                    atualizar_dag_g3_preencher_tabela_servidores();
                }
                else
                {
                    MessageBox.Show("Não é possível substituir o Servidor sem digitar o novo nome do Servidor. Preencha esse campo!");
                }
            }
            else
            {
                MessageBox.Show("Não é possível substituir o Servidor sem nome válido. Escolha uma opção!");
            }
        }



        private void btn_g2_adicionar_item_Click(object sender, EventArgs e)
        {
            if (txb_g2_endereco_remover.Text != "")
            {
                if (txb_g2_tag_adicionar.Text != "")
                {
                    if (cmb_g2_tipo_adicionar_item.Text != "")
                    {
                        string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                        SqlConnection con = new SqlConnection(connectionString);
                        try
                        {
                            con.Open();
                            txb_g2_status.Text = "Deu Certo!!!";

                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandText = "Insert into tblItens([endereco_item] , [tag], [servidor], [tipo_valor]) values('" + txb_g2_endereco_remover.Text + "','" + txb_g2_tag_adicionar.Text + "','" + cmb_g2_servidor.Text + "','" + cmb_g2_tipo_adicionar_item.Text + "')";

                            try
                            {

                                cmd.ExecuteNonQuery();
                                txb_g2_status.Text = "Item Adicionado!";
                                MessageBox.Show("Item adicionado com sucesso no servidor: '" + cmb_g2_servidor.Text + "'!");

                                //--dentro de Adicionar Usuário---- descobrindo o ID do item adicionado
                                string connectionString3 = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                                SqlConnection con3 = new SqlConnection(connectionString3);
                                SqlCommand cmd3 = new SqlCommand("Select id from tblItens where endereco_item='" + txb_g2_endereco_remover.Text + "' and servidor='" + cmb_g2_servidor.Text + "';", con3);
                                try
                                {
                                    con3.Open();
                                    int_id_usuario = (int)cmd3.ExecuteScalar();
                                    MessageBox.Show(int_id_usuario.ToString());
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("buscar o id não não deu certo");
                                }


                                txb_g2_endereco_remover.Text = "";
                                txb_g2_tag_adicionar.Text = "";
                                cmb_g2_tipo_adicionar_item.Text = "";

                                atualizar_cmb_g2_item_e_endereco();
                                atualizar_dag_g2_preencher_tabela_itens();

                                str_nome_tabela_valor = int_id_usuario.ToString();

                                //------- Criando a Tabela de Valores ------//
                                try
                                {
                                    string connectionString2 = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                                    SqlConnection con2 = new SqlConnection(connectionString2);
                                    SqlCommand cmd2 = new SqlCommand("CREATE TABLE [dbo].[tblValor" + int_id_usuario.ToString() + "]([data_time] [datetime] NOT NULL,[valor] [int] NOT NULL) ON [PRIMARY]; ALTER TABLE [dbo].[tblValor" + int_id_usuario.ToString() + "] ADD  CONSTRAINT [DF_tblValor" + int_id_usuario.ToString() + "_data_time]  DEFAULT (getdate()) FOR [data_time];", con2);

                                    SqlDataReader myReader2;

                                    con2.Open();
                                    myReader2 = cmd2.ExecuteReader();
                                    con2.Close();
                                    MessageBox.Show("Tabela tblValor" + int_id_usuario.ToString() + " criada com sucesso!");
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("A tabela tblValor" + int_id_usuario.ToString() + " não foi criada com sucesso.");
                                }




                            }
                            catch (Exception)
                            {
                                txb_g2_status.Text = "Não foi possível adicionar o item.";
                                MessageBox.Show("Este endereço de Item JÁ EXISTE nesse servidor. Tente outro endereço.");
                            }
                        }
                        catch (Exception)
                        {
                            txb_g2_status.Text = "Não deu certo.";
                            MessageBox.Show("Erro SQL ao tentar adicionar o item.");
                        }



                    }
                    else
                    {
                        MessageBox.Show("Não é possível adicionar um item sem TIPO válido. Preencha esse campo!");
                    }
                }
                else
                {
                    MessageBox.Show("Não é possível adicionar um item sem TAG válida. Preencha esse campo!");
                }
            }
            else
            {
                MessageBox.Show("Não é possível adicionar um item sem ENDEREÇO válido. Preencha esse campo!");
            }




        }

        private void btn_g2_remover_item_Click(object sender, EventArgs e)
        {
            if (cmb_g2_endereco_remover_item.Text != "")
            {
                string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                SqlConnection con = new SqlConnection(connectionString);
                try
                {
                    con.Open();
                    txb_g2_status.Text = "Deu Certo!!!";

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "Delete from tblItens where endereco_item='" + cmb_g2_endereco_remover_item.Text + "'";

                    try
                    {



                        //--dentro de Remover Usuário---- descobrindo o ID do item a ser removido
                        string connectionString3 = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                        SqlConnection con3 = new SqlConnection(connectionString3);
                        SqlCommand cmd3 = new SqlCommand("Select id from tblItens where endereco_item='" + cmb_g2_endereco_remover_item.Text + "' and servidor='" + cmb_g2_servidor.Text + "';", con3);
                        try
                        {
                            con3.Open();
                            int_id_usuario = (int)cmd3.ExecuteScalar();
                            MessageBox.Show(int_id_usuario.ToString());
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("buscar o id não não deu certo");
                        }


                        //------- Removendo a Tabela de Valores ------//
                        try
                        {
                            string connectionString2 = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                            SqlConnection con2 = new SqlConnection(connectionString2);
                            SqlCommand cmd2 = new SqlCommand("DROP TABLE [dbo].[tblValor" + int_id_usuario.ToString() + "]", con2);

                            SqlDataReader myReader2;

                            con2.Open();
                            myReader2 = cmd2.ExecuteReader();
                            con2.Close();
                            MessageBox.Show("Tabela tblValor" + int_id_usuario.ToString() + " removida com sucesso!");
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("A tabela tblValor" + int_id_usuario.ToString() + " não foi removida com sucesso.");
                        }

                        //----------Deletando efetivamente a Tabela tblItens-----------------//
                        cmd.ExecuteNonQuery();
                        txb_g2_status.Text = "Item removido!";
                        MessageBox.Show("Item removido com sucesso!");

                    }
                    catch (Exception)
                    {
                        txb_g2_status.Text = "Não removeu o Item.";
                        MessageBox.Show("Não foi possível remover o Item!");
                    }
                }
                catch (Exception)
                {
                    txb_g2_status.Text = "Não deu certo.";
                    MessageBox.Show("Erro SQL ao tentar remover o Item.");
                }

                cmb_g2_endereco_remover_item.Text = "";
                cmb_g2_tag_remover_item.Text = "";
                txb_g2_tipo_remover_item.Text = "";

                atualizar_cmb_g2_item_e_endereco();
                atualizar_dag_g2_preencher_tabela_itens();
            }
            else
            {
                MessageBox.Show("Não é possivel remover um item sem escolher o ENDEREÇO do item a ser removido.");
            }
        }


        private void btn_g2_substituir_item_Click(object sender, EventArgs e)
        {
            if(cmb_g2_endereco_substituir_item.Text != "")
            {
                if (txb_g2_endereco_substituir_item.Text != "")
                {
                    if (txb_g2_tag_substituir_item.Text != "")
                    {
                        if (txn_g2_tipo_substituir_item_novo.Text != "")
                        {

                            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                            SqlConnection con = new SqlConnection(connectionString);
                            try
                            {
                                con.Open();
                                txb_g2_status.Text = "Deu Certo!!!";

                                SqlCommand cmd = con.CreateCommand();
                                cmd.CommandText = "Update tblItens set endereco_item='" + txb_g2_endereco_substituir_item.Text + "', tag='" + txb_g2_tag_substituir_item.Text + "', servidor='" + cmb_g2_servidor.Text + "', tipo_valor='" + txn_g2_tipo_substituir_item_novo.Text + "'  where endereco_item='" + cmb_g2_endereco_substituir_item.Text + "' ;";

                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    txb_g2_status.Text = "Item substituído!";
                                    MessageBox.Show("Item substituido com sucesso!");
                                }
                                catch (Exception)
                                {
                                    txb_g2_status.Text = "Não substituiu o Item.";
                                    MessageBox.Show("Não foi possível substituir o Item.");
                                }

                            }
                            catch (Exception)
                            {
                                txb_g2_status.Text = "Não deu certo.";
                                MessageBox.Show("Erro SQL ao tentar substituir o Item.");
                            }

                            cmb_g2_endereco_substituir_item.Text = "";
                            cmb_g2_tag_substituir_item.Text = "";
                            txb_g2_tipo_substituir_item_antigo.Text = "";
                            txb_g2_endereco_substituir_item.Text = "";
                            txb_g2_tag_substituir_item.Text = "";
                            txn_g2_tipo_substituir_item_novo.Text = "";

                            atualizar_cmb_g2_item_e_endereco();
                            atualizar_dag_g2_preencher_tabela_itens();
                        }
                        else
                        {
                            MessageBox.Show("Não é possível substituir o Item sem o novo Tipo da Variável. Preencha esse campo!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Não é possível substituir o Item sem o novo TAG. Preencha esse campo!");
                    }
                }
                else
                {
                    MessageBox.Show("Não é possível substituir o Item sem o novo ENDEREÇO. Preencha esse campo!");
                }
            }
            else
            {
                MessageBox.Show("Não é possível substituir o Item sem ENDEREÇO válido. Escolha uma opção!");
            }
        }

        private void cmb_g2_servidor_SelectedIndexChanged(object sender, EventArgs e)
        {
            lab_g2_nome_do_servidor.Text = cmb_g2_servidor.Text;
            atualizar_dag_g2_preencher_tabela_itens();
            atualizar_cmb_g2_item_e_endereco();
        }

        private void cmb_g2_endereco_remover_item_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select tag from tblItens where endereco_item='" + cmb_g2_endereco_remover_item.Text + "' ;", con);
            SqlCommand cmd2 = new SqlCommand("Select tipo_valor from tblItens where endereco_item='" + cmb_g2_endereco_remover_item.Text + "' ;", con);

            try
            {
                con.Open();
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlDataAdapter da2 = new SqlDataAdapter();
                da.SelectCommand = cmd1;
                da2.SelectCommand = cmd2;
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                da.Fill(ds, "tag");
                da2.Fill(ds2, "tipo_valor");
                string senha_encontrada = "";
                string senha_encontrada2 = "";

                senha_encontrada = ((string)cmd1.ExecuteScalar());
                senha_encontrada2 = ((string)cmd2.ExecuteScalar());
                cmb_g2_tag_remover_item.Text = senha_encontrada;
                txb_g2_tipo_remover_item.Text = senha_encontrada2;
                con.Close();

            }
            catch (Exception)
            {
            }

        }

        private void cmb_g2_tag_remover_item_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select endereco_item from tblItens where tag='" + cmb_g2_tag_remover_item.Text + "' ;", con);
            SqlCommand cmd2 = new SqlCommand("Select tipo_valor from tblItens where tag='" + cmb_g2_tag_remover_item.Text + "' ;", con);

            try
            {
                con.Open();
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlDataAdapter da2 = new SqlDataAdapter();
                da.SelectCommand = cmd1;
                da2.SelectCommand = cmd2;
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                da.Fill(ds, "endereco_item");
                da2.Fill(ds2, "tipo_valor");
                string senha_encontrada = "";
                string senha_encontrada2 = "";

                senha_encontrada = ((string)cmd1.ExecuteScalar());
                senha_encontrada2 = ((string)cmd2.ExecuteScalar());
                cmb_g2_endereco_remover_item.Text = senha_encontrada;
                txb_g2_tipo_remover_item.Text = senha_encontrada2;
                con.Close();

            }
            catch (Exception)
            {
            }
        }

        private void cmb_g2_endereco_substituir_item_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select tag from tblItens where endereco_item='" + cmb_g2_endereco_substituir_item.Text + "' ;", con);
            SqlCommand cmd2 = new SqlCommand("Select tipo_valor from tblItens where endereco_item='" + cmb_g2_endereco_substituir_item.Text + "' ;", con);

            try
            {
                con.Open();
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlDataAdapter da2 = new SqlDataAdapter();
                da.SelectCommand = cmd1;
                da2.SelectCommand = cmd2;
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                da.Fill(ds, "tag");
                da2.Fill(ds2, "tipo_valor");
                string senha_encontrada = "";
                string senha_encontrada2 = "";

                senha_encontrada = ((string)cmd1.ExecuteScalar());
                senha_encontrada2 = ((string)cmd2.ExecuteScalar());
                cmb_g2_tag_substituir_item.Text = senha_encontrada;
                txb_g2_tipo_substituir_item_antigo.Text = senha_encontrada2;
                con.Close();

            }
            catch (Exception)
            {
            }
        }

        private void cmb_g2_tag_substituir_item_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select endereco_item from tblItens where tag='" + cmb_g2_tag_substituir_item.Text + "' ;", con);
            SqlCommand cmd2 = new SqlCommand("Select tipo_valor from tblItens where tag='" + cmb_g2_tag_substituir_item.Text + "' ;", con);

            try
            {
                con.Open();
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlDataAdapter da2 = new SqlDataAdapter();
                da.SelectCommand = cmd1;
                da2.SelectCommand = cmd2;
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                da.Fill(ds, "endereco_item");
                da2.Fill(ds2, "tipo_valor");
                string senha_encontrada = "";
                string senha_encontrada2 = "";

                senha_encontrada = ((string)cmd1.ExecuteScalar());
                senha_encontrada2 = ((string)cmd2.ExecuteScalar());

                cmb_g2_endereco_substituir_item.Text = senha_encontrada;
                txb_g2_tipo_substituir_item_antigo.Text = senha_encontrada2;
                con.Close();

            }
            catch (Exception)
            {
            }
        }

        //------------ Preencher tabelas --------------

        void atualizar_dag_g5_preencher_tabela_usuarios()
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);

            string strSQL = "select * from tblUsuarios";
            SqlCommand objcommand = new SqlCommand(strSQL, con);


            try
            {

                SqlDataAdapter objAdp = new SqlDataAdapter(objcommand);

                DataTable dtLista = new DataTable();

                objAdp.Fill(dtLista);

                dag_g5_lista_usuarios.DataSource = dtLista;
            }
            catch (Exception)
            {

            }
        }

        void atualizar_dag_g3_preencher_tabela_servidores()
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);

            string strSQL = "select * from tblServidores";
            SqlCommand objcommand = new SqlCommand(strSQL, con);


            try
            {

                SqlDataAdapter objAdp = new SqlDataAdapter(objcommand);

                DataTable dtLista = new DataTable();

                objAdp.Fill(dtLista);

                dag_g3_tabela_servidores.DataSource = dtLista;
            }
            catch (Exception)
            {

            }
        }

        void atualizar_dag_g2_preencher_tabela_itens()
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);

            string strSQL = "select * from tblItens where servidor='" + cmb_g2_servidor.Text + "';";
            SqlCommand objcommand = new SqlCommand(strSQL, con);


            try
            {

                SqlDataAdapter objAdp = new SqlDataAdapter(objcommand);

                DataTable dtLista = new DataTable();

                objAdp.Fill(dtLista);

                dag_g2_preencher_tabela_itens.DataSource = dtLista;
            }
            catch (Exception)
            {
                
            }
        }


        void atualizar_dag_g1_preencher_tabela_servidores_conectados()
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);

            string strSQL = "select * from tblServidores where servidor='" + cmb_g1_servidor.Text + "' and status_conexao=1;";
            SqlCommand objcommand = new SqlCommand(strSQL, con);
            

            try
            {

                SqlDataAdapter objAdp = new SqlDataAdapter(objcommand);

                DataTable dtLista = new DataTable();

                objAdp.Fill(dtLista);

                dag_g2_preencher_tabela_itens.DataSource = dtLista;
            }
            catch (Exception)
            {

            }
        }


        //------------ Atualizar ComboBox --------------

        void atualizar_cmb_g5_login_e_senha()
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select * from tblUsuarios;", con);
            SqlDataReader myReader;

            try
            {
                con.Open();
                myReader = cmd1.ExecuteReader();
                cmb_g5_login_remover.Items.Clear();
                cmb_g5_login_substituir_usuario.Items.Clear();


                while (myReader.Read())
                {
                    //string sName = (myReader["login"]);
                    cmb_g5_login_remover.Items.Add(myReader["login"]);
                    cmb_g5_login_substituir_usuario.Items.Add(myReader["login"]);

                }
            }
            catch (Exception)
            {

            }

        }

        void atualizar_cmb_g3_login_e_senha() // e tbm o ComboBox do grupo 2 Itens OPC
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select * from tblServidores;", con);
            SqlDataReader myReader;

            try
            {
                con.Open();
                myReader = cmd1.ExecuteReader();
                cmb_g3_servidores_remover.Items.Clear();
                cmb_g3_substituir_servidor.Items.Clear();
                cmb_g2_servidor.Items.Clear();


                while (myReader.Read())
                {
                    cmb_g3_servidores_remover.Items.Add(myReader["nome_servidor"]);
                    cmb_g3_substituir_servidor.Items.Add(myReader["nome_servidor"]);
                    cmb_g2_servidor.Items.Add(myReader["nome_servidor"]);

                }
            }
            catch (Exception)
            {

            }
        }

        void atualizar_cmb_g2_item_e_endereco()
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select * from tblItens where servidor='" + cmb_g2_servidor.Text + "';", con);
            SqlDataReader myReader;

            try
            {
                con.Open();
                myReader = cmd1.ExecuteReader();
                cmb_g2_endereco_remover_item.Items.Clear();
                cmb_g2_tag_remover_item.Items.Clear();
                cmb_g2_endereco_substituir_item.Items.Clear();
                cmb_g2_tag_substituir_item.Items.Clear();



                while (myReader.Read())
                {
                    cmb_g2_endereco_remover_item.Items.Add(myReader["endereco_item"]);
                    cmb_g2_tag_remover_item.Items.Add(myReader["tag"]);
                    cmb_g2_endereco_substituir_item.Items.Add(myReader["endereco_item"]);
                    cmb_g2_tag_substituir_item.Items.Add(myReader["tag"]);

                }
            }
            catch (Exception)
            {

            }
        }


        void atualizar_cmb_g1_servidores() // e tbm o ComboBox do grupo 2 Itens OPC
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("Select * from tblServidores;", con);
            SqlDataReader myReader;

            try
            {
                con.Open();
                myReader = cmd1.ExecuteReader();
                cmb_g1_servidor.Items.Clear();

                while (myReader.Read())
                {
                    cmb_g1_servidor.Items.Add(myReader["nome_servidor"]);

                }
            }
            catch (Exception)
            {

            }
        }

        //------------ Preencher comboBox do g2 Tipo com tipo de valores de itens (binário, int, float)---------(sem banco de dados)-------

        void preencher_cmb_g2_tipo_de_valores_definidos()
        {
            cmb_g2_tipo_adicionar_item.Items.Clear();
            cmb_g2_tipo_adicionar_item.Items.Add("int");
            cmb_g2_tipo_adicionar_item.Items.Add("binario");
            cmb_g2_tipo_adicionar_item.Items.Add("float");
            cmb_g2_tipo_adicionar_item.Items.Add("decimal");
            cmb_g2_tipo_adicionar_item.Items.Add("string");
        }

        private void button4_Click(object sender, EventArgs e) // Botão de Construir banco da dados
        {

            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con1 = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("create database dbPims;", con1);

            SqlDataReader myReader;
            try
            {
                con1.Open();
                myReader = cmd1.ExecuteReader();
                con1.Close();
                MessageBox.Show("O banco de dados 'dbPims foi criado com sucesso!");
                txb_g4_banco_de_dados.Text = "dbPims";


                try // Criando a Tabela  Usuários
                {
                    string connectionString2 = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=dbPims;User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                    SqlConnection con2 = new SqlConnection(connectionString2);
                    SqlCommand cmd2 = new SqlCommand("CREATE TABLE [dbo].[tblUsuarios]([login] [varchar](50) NOT NULL, [senha] [varchar](25) NOT NULL, [data_criacao] [date] NOT NULL, [data_ultimo_login] [date] NULL, [cargo] [varchar](50) NULL, CONSTRAINT [PK_tblUsuarios] PRIMARY KEY CLUSTERED ([login] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] ) ON [PRIMARY]; SET ANSI_PADDING OFF; ALTER TABLE [dbo].[tblUsuarios] ADD  CONSTRAINT [DF_tblUsuarios_data_criacao]  DEFAULT (getdate()) FOR [data_criacao];", con2);
                    
                    SqlDataReader myReader2;

                    con2.Open();
                    myReader2 = cmd2.ExecuteReader();
                    con2.Close();
                    int_criar_tblUsuario = 1;
                    MessageBox.Show("Tabela Usuários criada!");
                }
                catch (Exception)
                {
                    MessageBox.Show("A tabela Usuários não foi criada.");
                } // Fim da criação da Tabela Usuários


                try // Criando a Tabela Servidores
                {
                    string connectionString2 = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=dbPims;User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                    SqlConnection con2 = new SqlConnection(connectionString2);
                    SqlCommand cmd2 = new SqlCommand("CREATE TABLE [dbo].[tblServidores]([nome_servidor] [varchar](100) NOT NULL, [status_conexao] [int] NULL, CONSTRAINT [PK_tblServidores] PRIMARY KEY CLUSTERED ([nome_servidor] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", con2);
                    SqlDataReader myReader2;

                    con2.Open();
                    myReader2 = cmd2.ExecuteReader();
                    con2.Close();
                    int_criar_tblUsuario = 1;
                    MessageBox.Show("Tabela Servidores criada!");
                }
                catch (Exception)
                {
                    MessageBox.Show("A tabela Servidores não foi criada.");
                } // Fim da criação da Tabela Servidores


                try // Criando a Tabela Itens
                {
                    string connectionString2 = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=dbPims;User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                    SqlConnection con2 = new SqlConnection(connectionString2);
                    SqlCommand cmd2 = new SqlCommand("CREATE TABLE [dbo].[tblItens]([id] [int] IDENTITY(1,1) NOT NULL, [endereco_item] [varchar](100) NOT NULL,	[tag] [varchar](50) NOT NULL, [servidor] [varchar](100) NOT NULL, [tipo_valor] [varchar](15) NOT NULL, CONSTRAINT [PK_tblItens] PRIMARY KEY CLUSTERED ([endereco_item] ASC, [servidor] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", con2);
                    SqlDataReader myReader2;

                    con2.Open();
                    myReader2 = cmd2.ExecuteReader();
                    con2.Close();
                    int_criar_tblUsuario = 1;
                    MessageBox.Show("Tabela Itens criada!");
                }
                catch (Exception)
                {
                    MessageBox.Show("A tabela Itens não foi criada.");
                } // Fim da criação da Tabela Itens





                /*try // Criando a Tabela temperatura
                {
                    string connectionString2 = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=dbPims;User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                    SqlConnection con2 = new SqlConnection(connectionString2);
                    SqlCommand cmd2 = new SqlCommand("CREATE TABLE [dbo].[tbl"+1111+"]([id] [int] IDENTITY(1,1) NOT NULL, [endereco_item] [varchar](100) NOT NULL,	[tag] [varchar](50) NOT NULL, [servidor] [varchar](100) NOT NULL, [tipo_valor] [varchar](15) NOT NULL, CONSTRAINT [PK_tblItens] PRIMARY KEY CLUSTERED ([endereco_item] ASC, [servidor] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", con2);
                    SqlDataReader myReader2;

                    con2.Open();
                    myReader2 = cmd2.ExecuteReader();
                    con2.Close();
                    int_criar_tblUsuario = 1;
                    MessageBox.Show("Tabela temperatura criada!");
                }
                catch (Exception)
                {
                    MessageBox.Show("A tabela temperatura não foi criada.");
                } // Fim da criação da Tabela temperatura

                */

            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível criar o banco de dados.");
            }
            
                

                


           /* string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("create database dbPims;", con);
            

            SqlDataReader myReader;

            try
            {
                con.Open();
                MessageBox.Show("A conexão foi realizada com sucesso!");

                try
                {
                    myReader = cmd1.ExecuteReader();
                    con.Close();
                    MessageBox.Show("O banco de dados 'dbSilasCriar' foi criado com sucesso!");
                    

                    string connectionString2 = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog='"+textBox5.Text+"';User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
                    SqlConnection con2 = new SqlConnection(connectionString2);
                    SqlCommand cmd2 = new SqlCommand("CREATE TABLE [dbo].[tblAgoraDeu]([id] INT NOT NULL IDENTITY, [nome] varchar(50) NOT NULL, [agoradeu] varchar(100) NOT NULL);", con2);
                    SqlDataReader myReader2;
                    
                    try
                    {
                        con2.Open();
                        myReader2 = cmd2.ExecuteReader();
                        con2.Close();

                        MessageBox.Show("A tabela 'tblSilasCriacao' foi criada com sucesso!");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Não foi possível criar a tabela.");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Não foi possível criar o banco de dados.");
                }

            }
            catch(Exception)
            {
                MessageBox.Show("Não foi possível realizar a conexão.");
            }*/
            
        }

        private void button5_Click(object sender, EventArgs e) // Botão de criar tabela sql
        {
            /*
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);

            string sqlcreatetable = "CREATE TABLE pessoas(" +
                "id int IDENTITY(1,1) NOT NULL," +
                "nome nvarchar(50) NULL, " +
                "cidade nvarchar(50) NULL, " +
                "estado nchar(2) NULL, " +
                "PRIMARY KEY(id))";

            SqlCommand cmd = new SqlCommand(sqlcreatetable, con);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("A tabela foi criada com sucesso.");
            }
            catch (SqlException sqle)
            {
                MessageBox.Show("Não foi possível criar a tabela. Erro: " + sqle);
            }

            */

           ///*


            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand("CREATE TABLE [dbo].[tblSilasCriadissima]([id] INT NOT NULL IDENTITY, [nome] varchar(50) NOT NULL, [email] varchar(100) NOT NULL);", con);

            SqlDataReader myReader;

            try
            {
                con.Open();
                myReader = cmd1.ExecuteReader();
                MessageBox.Show("A tabela foi criada com sucesso!");

            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível criar a tabela.");
            }
            // */
        }

//------------------ CONECTAR OPC -------------------------------------------------------------------------//
        
        private void btn_g1_conectar_Click(object sender, EventArgs e)
        {

            if (cmb_g1_servidor.Text != "Escolha um Servidor OPC a ser conectado!") // Verificar se foi escolhido o servidor OPC (para se conectar a ele).
            {

                #region Constructor

                int taxaatualizacao = Convert.ToInt32(txb_g1_taxa_atualizacao.Text);
                tmr.Interval = TimeSpan.FromMilliseconds(taxaatualizacao);
                tmr.Tick += new EventHandler(tmr_Tick);
                tmr.Start();

                ConnectToOpcServer();


                #endregion
            } // Fim do if que verifica se o servidor OPC foi escolhido.
            else
            {
                MessageBox.Show("Escolha o Servidor OPC para realizar a conexão."); 
            }

        }


        #region GUI Update

        void tmr_Tick(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text +";");
            //SqlConnection con = new SqlConnection("Data Source=169.254.68.93\\SQLEXPRESS;Initial Catalog=dbPims;Persist Security Info=True;User ID=sa;Password=tanga123");
            
            // Armazenando no banco de dados os valores da cariável LSH_02
            try
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                if (LSH_02 == true)
                {
                    cmd.CommandText = "Insert into tblValor7([valor]) values(1)";
                    textBox1.Text = "1";
                }
                else
                {
                    cmd.CommandText = "Insert into tblValor7([valor]) values(0)";
                    textBox1.Text = "0";
                }

                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception)
                {
                    textBox2.Text = "Erro gravar bd";
                }

            }
            catch (Exception)
            {
                textBox1.Text = "Erro abrir db.";
            }



            // Armazenando no banco de dados os valores da cariável LSL_01
            try
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                if (LSL_01 == true)
                {
                    cmd.CommandText = "Insert into tblValor8([valor]) values(1)";
                    textBox4.Text = "1";
                }
                else
                {
                    cmd.CommandText = "Insert into tblValor8([valor]) values(0)";
                    textBox4.Text = "0";
                }

                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception)
                {
                    textBox2.Text = "Erro gravar bd";
                }

            }
            catch (Exception)
            {
                textBox4.Text = "Erro abrir db.";
            }


            //----- Mostrando a Temperatura (Valor) -----------
            textBox5.Text = Temperatura_TQ2.ToString();

            // Armazenando no banco de dados os valores da cariável LSL_01
            if (Temperatura_TQ2 > 0)
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandText = "Insert into tblValor15([valor]) values('" + Temperatura_TQ2 + "')";
                    try
                    {
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception)
                    {
                        txb_g3_status.Text = "Erro gravar bd";
                    }

                }
                catch (Exception)
                {
                    txb_g3_status.Text = "Erro abrir db.";
                }
            }


            //if (motorActive == true)
            //    ledMotor.Fill = new SolidColorBrush(Colors.Green);
            //else
            //    ledMotor.Fill = new SolidColorBrush(Colors.Gray);
            //
            //lblSpeed.Content = motorSpeed;
            //btnAutMan.IsChecked = autManSwitch;


    /////////        textBox1.Text = temperatura.ToString();
            
           // if (var_modificada == 1) // Se for o motor
           // {
                
    /*
                SqlConnection con = new SqlConnection("Data Source=SILAS-PC\\SQLEXPRESS;Initial Catalog=dbnovo;Persist Security Info=True;User ID=sa;Password=tanga123");
                try
                {
                    

                    con.Open();
                    textBox2.Text = "Deu Certo!!!";

                    SqlCommand cmd = con.CreateCommand();
                    if (motorActive == true)
                    {
                        cmd.CommandText = "Insert into tblMotor([valor]) values(1)";
                    }
                    else
                    {
                        cmd.CommandText = "Insert into tblMotor([valor]) values(0)";
                    }

                    try
                    {
                        cmd.ExecuteNonQuery();
                        textBox2.Text = "Dados gravados1!!!";
                        con.Close();
                    }
                    catch (Exception)
                    {
                        textBox2.Text = "Não gravou os dados1.";
                    }
                }
                catch (Exception)
                {
                    textBox2.Text = "Não deu certo1.";
                }
    */
           // }
           // else if (var_modificada == 2) // Se for a temperatura
           // {

    /*
                 con = new SqlConnection("Data Source=SILAS-PC\\SQLEXPRESS;Initial Catalog=dbnovo;Persist Security Info=True;User ID=sa;Password=tanga123");
                try
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "Insert into tblTemperatura([valor]) values('" + temperatura + "')";
                    cmd.ExecuteNonQuery();
                    textBox2.Text = "Dados gravados2.";
                    con.Close();

                }
                catch (Exception)
                {
                    textBox2.Text = "Não deu certo2";
                }
            //}
    */
            

/*

            if (motorActive == true)
            { //
                textBox1.Text = "1"; //

                con = new SqlConnection("Data Source=SILAS-PC\\SQLEXPRESS;Initial Catalog=dbnovo;Persist Security Info=True;User ID=sa;Password=tanga123");
                try
                {
                    con.Open();
                    textBox2.Text = "Deu Certo!!!";

                    cmd = con.CreateCommand();
                    cmd.CommandText = "Insert into tblpims([tag] , [valor]) values('motor',1)";

                    try
                    {
                        cmd.ExecuteNonQuery();
                        textBox2.Text = "Dados Gravados!";
                    }
                    catch (Exception)
                    {
                        textBox2.Text = "Não gravou os dados.";
                    }
                }
                catch (Exception)
                {
                    textBox2.Text = "Não deu certo.";
                }
                
            }
            else //
            {
                textBox1.Text = "0"; //

                ////SqlConnection con = new SqlConnection("Integrated Security=SSPI;Data Source=MAURINHO-VIRTUA\\SQLEXPRESS;Persist Security Info=False;User ID=sa;Initial Catalog=dbnovo");
                try
                {
                    con.Open();
                    textBox2.Text = "Deu Certo!!!";

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "Insert into tblpims([tag] , [valor]) values('motor',0)";

                    try
                    {
                        cmd.ExecuteNonQuery();
                        textBox2.Text = "Dados Gravados!";
                    }
                    catch (Exception)
                    {
                        textBox2.Text = "Não gravou os dados.";
                    }
                }
                catch (Exception)
                {
                    textBox2.Text = "Não deu certo.";
                }
                // *

            } */

            var_modificada = 0;
        }

        #endregion

        

        #region OPC Connection and Data Updated callback

        private void ConnectToOpcServer()
        {
            // 1st: Create a server object and connect to the RSLinx OPC Server
            try
            {
                server = new Opc.Da.Server(fact, null);
                //server.Url = new Opc.URL("opcda://localhost/RSLinx OPC Server");
                //server.Url = new Opc.URL("opcda://localhost/ICONICS.SimulatorOPCDA");
                server.Url = new Opc.URL("opcda://localhost/"+cmb_g1_servidor.Text+"");
                //server.Url = new Opc.URL("opcda://localhost/SimulatorOPCDA");
                
                //2nd: Connect to the created server
                server.Connect();
                MessageBox.Show("A conexão dom o Servidor OPC " + cmb_g1_servidor.Text + " foi realizada com sucesso!");
                statusConexaoSilas = 1;  // variável criada apenas para saber se foi conectado (para habilitar o botão "DESCONECTAR")
                prencher_na_tblServidores_um_de_conectado();
                atualizar_dag_g1_preencher_tabela_servidores_conectados();
                

                //Read group subscription
                groupState = new Opc.Da.SubscriptionState();
                groupState.Name = "myReadGroup";
                int taxaatualizacao = Convert.ToInt32(txb_g1_taxa_atualizacao.Text); // passando o valor de atualização do grupo (mesma coisa foi feita para o timer)
                groupState.UpdateRate = taxaatualizacao;
                groupState.Active = true;
                //Read group creation
                groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);
                groupRead.DataChanged += new Opc.Da.DataChangedEventHandler(groupRead_DataChanged);

                /*Item item = new Item();
                item.ItemName = "[MYPLC]N7:0";
                itemsList.Add(item);

                item = new Item();
                item.ItemName = "[MYPLC]O:0/0";
                itemsList.Add(item);

                item = new Item();
                item.ItemName = "[MYPLC]B3:0/3";
                itemsList.Add(item);*/

                Item item;

                item = new Item();                                   //
                //item.ItemName = "[TreinamentoBasico_2]Local:4:O.Data[0].0"; //
                item.ItemName = "Logical.Motor";
                itemsList.Add(item);                                      //

                item = new Item();
                item.ItemName = "Numeric.Temperatura";
                itemsList.Add(item);

                item = new Item();
                item.ItemName = "[1769-L24ER-QBFC1B/A_LOGIX5324ER]Program:MainProgram.LSH_02";
                itemsList.Add(item);

                item = new Item();
                item.ItemName = "[1769-L24ER-QBFC1B/A_LOGIX5324ER]Program:MainProgram.LSL_01";
                itemsList.Add(item);
                
                item = new Item();
                item.ItemName = "[1769-L24ER-QBFC1B/A_LOGIX5324ER]Program:MainProgram.Temp";
                itemsList.Add(item);

                



                groupRead.AddItems(itemsList.ToArray());

                groupStateWrite = new Opc.Da.SubscriptionState();
                groupStateWrite.Name = "myWriteGroup";
                groupStateWrite.Active = false;
                groupWrite = (Opc.Da.Subscription)server.CreateSubscription(groupStateWrite);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Não foi possível conectar ao Servidor OPC " + cmb_g1_servidor.Text + ". Servidor não existente no momento.");
            }
        }

        void groupRead_DataChanged(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
            foreach (ItemValueResult itemValue in values)
            {
                switch (itemValue.ItemName)
                {
                    /*case "[MYPLC]N7:0":
                        motorSpeed = Convert.ToInt32(itemValue.Value);
                        break;

                    case "[MYPLC]O:0/0":
                        motorActive = Convert.ToBoolean(itemValue.Value);
                        break;

                    case "[MYPLC]B3:0/3":
                        autManSwitch = Convert.ToBoolean(itemValue.Value);
                        break;*/

                    //case "[TreinamentoBasico_2]Local:4:O.Data[0].0":        //
                    case "Logical.Motor":
                        motorActive = Convert.ToBoolean(itemValue.Value); //
                        var_modificada = 1;
                        break;                                       //

                    case "Numeric.Temperatura":
                        //temperatura = Convert.ToInt32(itemValue.Value);
                        temperatura = Convert.ToInt32(itemValue.Value);
                        var_modificada = 2;
                        break;

                    case "[1769-L24ER-QBFC1B/A_LOGIX5324ER]Program:MainProgram.LSH_02":
                        //temperatura = Convert.ToInt32(itemValue.Value);
                        LSH_02 = Convert.ToBoolean(itemValue.Value);
                        var_modificada = 2;
                        break;

                    case "[1769-L24ER-QBFC1B/A_LOGIX5324ER]Program:MainProgram.LSL_01":
                        //temperatura = Convert.ToInt32(itemValue.Value);
                        LSL_01 = Convert.ToBoolean(itemValue.Value);
                        var_modificada = 2;
                        break;

                    case "[1769-L24ER-QBFC1B/A_LOGIX5324ER]Program:MainProgram.Temp":
                        //temperatura = Convert.ToInt32(itemValue.Value);
                        Temperatura_TQ2 = Convert.ToInt32(itemValue.Value);
                        var_modificada = 2;
                        break;
                    
                }
            }
        }

        #endregion


        #region Write Methods

        private void WriteData(string itemName, int value)
        {
            groupWrite.RemoveItems(groupWrite.Items);
            List<Item> writeList = new List<Item>();
            List<ItemValue> valueList = new List<ItemValue>();

            Item itemToWrite = new Item();
            itemToWrite.ItemName = itemName;
            ItemValue itemValue = new ItemValue(itemToWrite);
            itemValue.Value = value;

            writeList.Add(itemToWrite);
            valueList.Add(itemValue);
            //IMPORTANT:
            //#1: assign the item to the group so the items gets a ServerHandle
            groupWrite.AddItems(writeList.ToArray());
            // #2: assign the server handle to the ItemValue
            for (int i = 0; i < valueList.Count; i++)
                valueList[i].ServerHandle = groupWrite.Items[i].ServerHandle;
            // #3: write
            groupWrite.Write(valueList.ToArray());
        }

        private const int ON = 1;
        private const int OFF = 0;
        private void WritePushButton(string itemName)
        {
            groupWrite.RemoveItems(groupWrite.Items);
            List<Item> writeList = new List<Item>();
            List<ItemValue> valueList = new List<ItemValue>();

            Item itemToWrite = new Item();
            itemToWrite.ItemName = itemName;
            ItemValue itemValue = new ItemValue(itemToWrite);
            itemValue.Value = ON;

            writeList.Add(itemToWrite);
            valueList.Add(itemValue);
            //IMPORTANT:
            //#1: assign the item to the group so the items gets a ServerHandle
            groupWrite.AddItems(writeList.ToArray());
            // #2: assign the server handle to the ItemValue
            for (int i = 0; i < groupWrite.Items.Length; i++)
                valueList[i].ServerHandle = groupWrite.Items[i].ServerHandle;
            // #3: now write
            groupWrite.Write(valueList.ToArray());

            Thread.Sleep(200); /////////////////////////// Deixei esse tempo porque acho que é o tempo de verificação dos botões pressionados e esse tempo deve ser curto. Pois imediatamente após pressionar o botão quero que a informação seja passada para o servidor OPC.

            itemValue.Value = OFF;

            writeList.Add(itemToWrite);
            valueList.Add(itemValue);
            //IMPORTANT:
            //#1: assign the item to the group so the items gets a ServerHandle
            groupWrite.AddItems(writeList.ToArray());
            // #2: assign the server handle to the ItemValue
            for (int i = 0; i < valueList.Count; i++)
                valueList[i].ServerHandle = groupWrite.Items[i].ServerHandle;
            // #3: now write
            groupWrite.Write(valueList.ToArray());
        }

        #endregion

        

        

        
        #region Callbacks

        /*private void btnAutMan_Click(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(!btnAutMan.IsChecked);
            WriteData("[MYPLC]B3:0/3", value);
            e.Handled = true;
        }

        private void btnStartMotor_Click(object sender, RoutedEventArgs e)
        {
            WritePushButton("[MYPLC]B3:0/0");
        }

        private void btnStoptMotor_Click(object sender, RoutedEventArgs e)
        {
            WritePushButton("[MYPLC]B3:0/1");
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WriteData("[MYPLC]B3:0/4", ON);
        }

        private void Button_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WriteData("[MYPLC]B3:0/4", OFF);
        }*/
        
        
        private void button1_Click(object sender, EventArgs e) //botão Start Motor
        {
            WritePushButton("[TreinamentoBasico_2]Local:3:I.Data[1].0");
        }

        private void button2_Click(object sender, EventArgs e) //Botão Stop Motor
        {
            WritePushButton("[TreinamentoBasico_2]Local:3:I.Data[1].1");
        }
        

        #endregion

        private void btn_g1_desconectar_Click(object sender, EventArgs e)
        {

            if (statusConexaoSilas == 1)
            {
                MessageBox.Show("O Servidor OPC foi desconectado!");
                server.Disconnect();
                tmr.Stop();
                statusConexaoSilas = 0;
                prencher_na_tblServidores_zero_de_desconectado();
                atualizar_dag_g1_preencher_tabela_servidores_conectados();

            }
            else 
            {
                MessageBox.Show("Não há nenhum Servidor OPC Conectado.");
            }
            
        }

        private void groupBox1_Conexao_Enter(object sender, EventArgs e)
        {

        }

        private void cmb_g1_servidor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //atualizar_cmb_g1_servidores();
        }


      

        

        
                

        

  

        

//------------------ fim do CONECTAR OPC -----------------------------------------------------------------//       




        void prencher_na_tblServidores_um_de_conectado()
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Insert into tblServidores([status_conexao]) values(1) where nome_servidor='" + cmb_g1_servidor.Text+ "' ;";
                
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        void prencher_na_tblServidores_zero_de_desconectado()
        {
            string connectionString = @"Data Source=" + txb_g4_servidor_sql.Text + ";Initial Catalog=" + txb_g4_banco_de_dados.Text + ";User ID=" + txb_g4_login_sql.Text + ";Password=" + txb_g4_senha_sql.Text;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Insert into tblServidores([status_conexao]) values(0) where nome_servidor='" + cmb_g1_servidor.Text + "' ;";

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
           /*
            Opc.IDiscovery discovery = new OpcCom.ServerEnumerator();
            Opc.Server[] localServers = discovery.GetAvailableServers(Opc.Specification.COM_DA_20);

            cmb_g1_servidor.Items.Clear();

            foreach (Opc.Server found in localServers)
            {
                //txtServers.AppendText(string.Format("\r\n{0}", found.Name));

                cmb_g1_servidor.Items.Add(string.Format("\r\n{0}", found.Name));
            }
           */
        }

        void browser_servidor_OPC()
        {
            Opc.IDiscovery discovery = new OpcCom.ServerEnumerator();
            Opc.Server[] localServers = discovery.GetAvailableServers(Opc.Specification.COM_DA_20);

            comboBox1.Items.Clear();

            foreach (Opc.Server found in localServers)
            {
                //txtServers.AppendText(string.Format("\r\n{0}", found.Name));

                comboBox1.Items.Add(string.Format("{0}", found.Name)); //comboBox1.Items.Add(string.Format("\r\n{0}", found.Name));
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            /*Opc.IDiscovery discovery = new OpcCom.ServerEnumerator();
            Opc.Server[] localServers = discovery.GetAvailableServers(Opc.Specification.COM_DA_20);

            comboBox1.Items.Clear();
            
            foreach (Opc.Server found in localServers)
            {
                //txtServers.AppendText(string.Format("\r\n{0}", found.Name));

                comboBox1.Items.Add(string.Format("{0}", found.Name)); //comboBox1.Items.Add(string.Format("\r\n{0}", found.Name));
            }
             */
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txb_g3_servidor_adicionar.Text = comboBox1.SelectedItem.ToString();
        }

        
       
    }
}
// mudar: txn_g2_tipo_substituir_item_novo pois txn ta errado, o certo é txB


