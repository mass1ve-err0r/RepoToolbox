using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Markdig;
using RepoToolbox.Structs;

namespace RepoToolbox.Windows
{
    /// <summary>
    /// Interaktionslogik für MarkdownDepictionEditor.xaml
    /// </summary>
    public partial class MarkdownDepictionEditor : Window
    {
        private Package pkg;

        public MarkdownDepictionEditor(ref Package refpkg) {
            InitializeComponent();
            DepictionView.UseLayoutRounding = true;
            pkg = refpkg;
            if (!String.IsNullOrEmpty(pkg.DepictionMDRAW)) {
                DepictionText.Text = pkg.DepictionMDRAW;
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            AddPackage.isOpenDepictionEditor = false;
            base.OnClosing(e);
        }

        private void UpdateView(object sender, EventArgs e) {
            var mdText = DepictionText.Text;
            var mdhtml = Markdown.ToHtml(mdText);
            if (String.IsNullOrEmpty(mdhtml)) {
                // nothing
            } else {
                DepictionView.NavigateToString(mdhtml);
            }
        }

        private void CardSaveDepiction(object sender, MouseButtonEventArgs e) {
            pkg.DepictionMDRAW = DepictionText.Text;
            pkg.DepictionMD = ToLiteral(DepictionText.Text);
            // DEBUG File.WriteAllText(Environment.CurrentDirectory + "\\sampleMD.md", pkg.DepictionMD);
            pkg.DepictionHTML = Markdown.ToHtml(DepictionText.Text);
            // DEBUG File.WriteAllText(Environment.CurrentDirectory + "\\sample.html", pkg.DepictionHTML);
            this.Close();
        }

        private string ToLiteral(string Input) {
            using (var writer = new StringWriter()) {
                using (var provider = CodeDomProvider.CreateProvider("CSharp")) {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(Input), writer, null);
                    return writer.ToString().Replace(@"\r\n", @"\n");
                }
            }
        }

        // <-- FIN -->
    }
}
