using BriefingBuilder.Builder;
using BriefingBuilder.ContentPages;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BriefingBuilder
{
    public partial class MainForm : Form
    {
        private List<DesignerHostBase> _pages = new List<DesignerHostBase>();
        private int _currentPage = 0;
        private ContentPageBuilder _contentBuilder;

        public MainForm()
        {
            InitializeComponent();

            this.ClientSize = new Size(630, 580);

            //_pages.Add(new BriefingPage_Cover() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_Chapter1() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_Objective() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_Bootup() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_Chapter2() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_Overview() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_CurrentAttemptIndicator() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_CodeInput() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_AttemptHistory() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_SubmitButton() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_DeviceMenu() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_Chapter3() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_CodeOverview() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_ResultsOverview() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_Examples1() { Dock = DockStyle.Fill });
            //_pages.Add(new BriefingPage_Examples2() { Dock = DockStyle.Fill });

            UpdateContentPanel();

            _contentBuilder = new ContentPageBuilder();
        }

        private void UpdateContentPanel()
        {
            _pnlContent.Controls.Clear();
            _pnlContent.Controls.Add(_pages[_currentPage]);
        }

        private void Handle_GenerateButtonClick(object sender, System.EventArgs e)
        {
            foreach (IDesignerHost designerHost in _pages)
            {
                _contentBuilder.GenerateContentPage(designerHost, @"c:\temp");
            }
        }

        private void Handle_PreviousButtonClick(object sender, System.EventArgs e)
        {
            if (_currentPage > 0)
            {
                _currentPage -= 1;
                UpdateContentPanel();
            }
        }

        private void Handle_NextButtonClick(object sender, System.EventArgs e)
        {
            if (_currentPage < _pages.Count - 1)
            {
                _currentPage += 1;
                UpdateContentPanel();
            }
        }
    }
}
