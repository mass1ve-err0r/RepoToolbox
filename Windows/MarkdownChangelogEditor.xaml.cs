using RepoToolbox.Structs;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Markdig;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace RepoToolbox.Windows
{
    /// <summary>
    /// Interaktionslogik für MarkdownChangelogEditor.xaml
    /// </summary>
    public partial class MarkdownChangelogEditor : Window
    {
        Package pkg;

        public MarkdownChangelogEditor(ref Package refpkg) {
            InitializeComponent();
            ChangelogView.UseLayoutRounding = true;
            pkg = refpkg;
            if (!String.IsNullOrEmpty(pkg.ChangelogMDRAW)) {
                ChangelogText.Text = pkg.ChangelogMDRAW;
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);
        }

        private void UpdateView(object sender, EventArgs e) {
            var mdText = ChangelogText.Text;
            var mdhtml = Markdown.ToHtml(mdText);
            if (String.IsNullOrEmpty(mdhtml)) {
                // nothing
            }
            else {
                ChangelogView.NavigateToString(mdhtml);
            }
        }

        private void CardSaveChangelog(object sender, MouseButtonEventArgs e) {
            pkg.ChangelogMDRAW = ChangelogText.Text;
            pkg.ChangelogMD = ToLiteral(ChangelogText.Text);
            // DEBUG File.WriteAllText(Environment.CurrentDirectory + "\\sampleMD.md", pkg.DepictionMD);
            pkg.ChangelogHTML = Markdown.ToHtml(ChangelogText.Text);
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
    }
}
