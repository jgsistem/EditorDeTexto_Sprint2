using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;
using System.Diagnostics;
using System.Threading;

namespace EditorDeTexto_SW
//*****************************************************************************************************************************
//                        EDITOR DE TEXTO
//*****************************************************************************************************************************
   
{
    public partial class EditorTMain : Form
    {

        String RutaArchivo = null;

        public EditorTMain()
        {
            InitializeComponent();
            AllowDrop = true;
        
        DragDrop += new DragEventHandler(txtEditex_DragDrop);
        DragEnter += new DragEventHandler(txtEditex_DragEnter);
        }

        private void EditorTMain_Load(object sender, EventArgs e)
        {
            txtEditex.Focus();
           
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
           txtEditex.Clear();
           BarraName.Text = "";
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open = new OpenFileDialog();            
            Open.Filter = "Text [*.txt*]|*.txt|All Files [*,*]|*,*";
            Open.CheckFileExists = true;
            Open.Title = "Abrir Archivo";
            Open.ShowDialog(this);
            RutaArchivo = Open.FileName;  
            try
            {                
                using (StreamReader sr = new StreamReader(Open.FileName))
                {
                    String line;
                    string Lines = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Lines = line + "\r\n";
                        txtEditex.Text += Lines;

                    }
                                    
                }
                BarraName.Text = RutaArchivo;
               
            }
            catch (Exception){

            }
            finally{              
                Open.Dispose();
            }
        }

        void saveAs()
        {
            SaveFileDialog Save = new SaveFileDialog();
            System.IO.StreamWriter myStreamWriter = null;
            Save.Filter = "Text (*.txt)|*.txt|HTML(*.html*)|*.html|All files(*.*)|*.*";
            Save.CheckPathExists = true;
            Save.Title = "Guardar como";
            Save.ShowDialog(this);
            try
            {
                myStreamWriter = System.IO.File.AppendText(Save.FileName);
                myStreamWriter.Write(txtEditex.Text);
                myStreamWriter.Flush();
                RutaArchivo = Save.FileName;

            }
            catch (Exception) { }
            finally {
                BarraName.Text = RutaArchivo;
            }
        }

        void saveAsGuardar()
        {
            SaveFileDialog Save = new SaveFileDialog();
            System.IO.StreamWriter myStreamWriter = null;
            Save.Filter = "Text (*.txt)|*.txt|HTML(*.html*)|*.html|All files(*.*)|*.*";           
            try
            {
                myStreamWriter = System.IO.File.AppendText(RutaArchivo);
                myStreamWriter.Write(txtEditex.Text);
                myStreamWriter.Flush();

                RutaArchivo = Save.FileName;

            }
            catch (Exception) { }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fuenteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FontDialog font = new FontDialog();
            font.Font = txtEditex.Font;
            if (font.ShowDialog() == DialogResult.OK)
            {
                txtEditex.Font = font.Font;
            }
        }

        private void colorDeLetraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (color.ShowDialog() == DialogResult.OK)
            {
                txtEditex.ForeColor = color.Color;
            }
        }

        private void colorDeFondoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog fondo = new ColorDialog();
            if (fondo.ShowDialog() == DialogResult.OK)
            {
                txtEditex.BackColor = fondo.Color;
            }
        }

        private void EditorTMain_ResizeBegin(Object sender, EventArgs e)
        {

            MessageBox.Show("You are in the Form.ResizeBegin event.");

        }

        private void nosotrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nostros frm = new Nostros();
            frm.Show();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            System.IO.StreamWriter myStreamWriter = null;
            if (System.IO.File.Exists(RutaArchivo))
            {
                try
                {

                    using (StreamWriter sw = new StreamWriter(RutaArchivo))
                    {
                        sw.Write(txtEditex.Text);
                    }
                   
                }
                catch (Exception ex)
                {
                    Thread.Sleep(100);
                    saveAsGuardar();
                }
                finally {
                    BarraName.Text = RutaArchivo;
                }
            }                 
        }

        private void SendToPrinter(String filePath)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = filePath;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process p = new Process())
            {
                p.StartInfo = info;
                p.Start();
                p.WaitForInputIdle();
                System.Threading.Thread.Sleep(3000);
            }
           
        }

        private void txtEditex_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                String Nombre = null;
                DataObject data = (DataObject)e.Data;
                if (data.ContainsFileDropList())
                {
                    string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (rawFiles != null)
                    {
                        List<string> lines = new List<string>();
                        foreach (string path in rawFiles)
                        {
                            Nombre = path;
                            lines.AddRange(File.ReadAllLines(path));
                        }
                        BarraName.Text = Nombre;
                        txtEditex.Text = "";
                        var ListaArchivos = lines.ToArray();
                        if (ListaArchivos.Count() > 1)
                        {
                            String Lines = null;
                            foreach (string paths in ListaArchivos)
                            {
                                Lines = paths +  "\r\n";
                                txtEditex.Text += Lines;
                            }                            
                        }
                        else
                        {
                            foreach (string paths in ListaArchivos)
                            {
                                txtEditex.Text = paths;
                            }                          
                        } 
                    }
                }
            }
            catch (Exception)
            {                
                throw;
            }
            
        }

        private void txtEditex_DragEnter(object sender, DragEventArgs e)
        {
             e.Effect = DragDropEffects.Copy;
        }

        
   }    
    
}
