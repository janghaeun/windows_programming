using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace memo
{
    public partial class FormNotePad : Form
    {
        private bool changed = false;

        public FormNotePad()
        {
            InitializeComponent();
        }

        private void textBoxNote_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void saveTextToFile()
        {
            if(this.Text=="제목 없음")
            {
                if(saveFileDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    string str = saveFileDialog1.FileName;
                    var sw = new StreamWriter(str,false);
                    var text = this.textBoxNote.Text;
                    sw.Write(text);
                    sw.Flush();
                    sw.Close();

                    var f = new FileInfo(str);
                    this.Text = f.Name;
                }
            }
            else
            {
                string str = saveFileDialog1.FileName;
                var sw = new StreamWriter(str, false);

                var text = this.textBoxNote.Text;
                sw.Write(text);
                sw.Flush();
                sw.Close();

                var f = new FileInfo(str);
                this.Text = f.Name;
            }
        }

        private void 새로만들기NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.changed != false)
            {
                var msg = MessageBox.Show("변경 내용을 제목 없음으로 저장하시겠습니까?", "메모장", MessageBoxButtons.YesNoCancel);
                if (msg == DialogResult.Yes)
                {
                    saveTextToFile();

                    this.Text = "제목 없음";
                    this.textBoxNote.ResetText();
                    this.changed = false;
                }
                else if(msg == DialogResult.No)
                {
                    this.Text = "제목 없음";
                    this.textBoxNote.ResetText();
                    this.changed = false;
                }
                else if(msg == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                this.textBoxNote.ResetText();
                this.Text = "제목 없음";
                this.changed = false;
            }   
        }

        private void 열기OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                string str = openFileDialog1.FileName;
                var sr = new StreamReader(str);

                this.textBoxNote.Text = sr.ReadToEnd();
                sr.Close();

                var f = new FileInfo(str);
                this.Text = f.Name;
                this.changed = false;
            }
        }

        private void 저장SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveTextToFile();
            this.changed = false;
        }

        private void 다른이름으로저장AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                string str = saveFileDialog1.FileName;
                var sw = new StreamWriter(str, false);
                var text = this.textBoxNote.Text;
                sw.Write(text);
                sw.Flush();
                sw.Close();

                var f = new FileInfo(str);
                this.Text = f.Name;

                this.changed = false;
            }
        }

        private void FormNotePad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(this.changed != false)
            {
                e.Cancel = true;
                var msg = MessageBox.Show("변경 내용을 제목 없음으로 저장하시겠습니까?", "메모장", MessageBoxButtons.YesNoCancel);
                if(msg == DialogResult.Yes)
                {
                    saveTextToFile();
                }
                else if(msg == DialogResult.No)
                {
                    this.Dispose();
                }else if(msg == DialogResult.Cancel)
                {
                    return;
                }
            }
            
        }

        private void 끝내기XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 실행취소UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBoxNote.Undo();
        }

        private void 잘라내기XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBoxNote.Cut();
        }

        private void 복사CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBoxNote.Copy();
        }

        private void 붙여넣기VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBoxNote.Paste();
        }

        private void 삭제DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBoxNote.Text = "";
        }

        private void 모두선택AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBoxNote.SelectAll();
        }

        private void 시간날짜TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 자동줄바꿈WToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 글꼴FToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private string strFind;
        private FormFind formFind;

        private void FormFindNextButton_Click(object sender, EventArgs e)
        {
            var updown = -1;
            var str = this.textBoxNote.Text;
            var findword = formFind.textBoxWord.Text;

            if(formFind.checkBoxCap.Checked == false) {
                str = str.ToUpper();
                findword = findword.ToUpper();
            }

            if(formFind.radioButtonUp.Checked == true)
            {
                if(textBoxNote.SelectionStart != 0)
                {
                    updown = str.LastIndexOf(findword,textBoxNote.SelectionStart-1);
                }

            }
            else
            {
                updown = str.IndexOf(findword, textBoxNote.SelectionStart + textBoxNote.SelectionLength);
            }
            if(updown == -1)
            {
                MessageBox.Show("'" + findword + "'를 찾을 수 없습니다.", "메모장", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            { 
                this.textBoxNote.Select(updown,findword.Length);
                this.textBoxNote.Focus();
                this.textBoxNote.ScrollToCaret();
            }

        }

        private void 찾기FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!(formFind == null || formFind.Visible == false))
            {
                formFind.Focus();
                return;
            }
            formFind = new FormFind();

            if (textBoxNote.SelectionLength == 0)
                formFind.textBoxWord.Text = strFind;
            else
            {
                formFind.textBoxWord.Text = textBoxNote.SelectedText;
            }
            formFind.buttonNext.Click += new System.EventHandler(FormFindNextButton_Click);
            formFind.Show();
        }
    }
}
