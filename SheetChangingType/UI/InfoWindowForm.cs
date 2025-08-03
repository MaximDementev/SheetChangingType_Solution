using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;
using MessageBox = System.Windows.Forms.MessageBox;
using RichTextBox = System.Windows.Forms.RichTextBox;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;

namespace KRGPMagic.Plugins.SheetChangingType.UI
{
    public class InfoWindowForm : Form
    {
        private RichTextBox richTextBox;

        public InfoWindowForm(string text, IEnumerable<string> links)
        {
            InitializeComponent();
            PopulateContent(text, links);
        }

        private void InitializeComponent()
        {
            // Form settings
            this.Text = "Справка: Тип ИЗМа";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.Padding = new Padding(10);

            // RichTextBox settings
            this.richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F),
                DetectUrls = true // Automatically detect URLs
            };

            // Event handler for clicking links
            this.richTextBox.LinkClicked += RichTextBox_LinkClicked;

            // Add control to form
            this.Controls.Add(this.richTextBox);
        }

        private void PopulateContent(string text, IEnumerable<string> links)
        {
            // Add main text
            richTextBox.Text = text;

            // Add links section if any exist
            bool hasLinks = false;
            foreach (var link in links)
            {
                if (!string.IsNullOrWhiteSpace(link))
                {
                    hasLinks = true;
                    break;
                }
            }

            if (hasLinks)
            {
                richTextBox.AppendText("\n\nСсылки:\n");
                foreach (string link in links)
                {
                    if (!string.IsNullOrWhiteSpace(link))
                    {
                        richTextBox.AppendText(link + "\n");
                    }
                }
            }
        }

        private void RichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                // More reliable way to open URLs
                var psi = new ProcessStartInfo
                {
                    FileName = e.LinkText,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
