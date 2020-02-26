using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.YY.UI.M
{

    public partial class Pager : System.Web.UI.UserControl
    {

        public delegate void OnPager(int pageSize, int pageIndex);
        public event OnPager OnPrevPager;
        public event OnPager OnNextPager;
        public event OnPager OnLastPager;
        public event OnPager OnFirstPager;
        public event OnPager OnGotoPager;
        public int RowCount { get; set; }
        public int PageIndex
        {
            get
            {
                int _pageIndex = 1;
                int.TryParse(currentPage.Value, out _pageIndex);
                return _pageIndex;
            }
            set
            {
                if (value > 0 && value <= PageCount)
                    currentPage.Value = value.ToString();
                else
                    currentPage.Value = "1";
            }
        }
        public int PageSize
        {
            get
            {
                int _pageSize = 30;
                int.TryParse(txtPageSize.Text, out _pageSize);
                return _pageSize;
            }
            set
            {
                if (value < 0 || value > 10000)
                    txtPageSize.Text = "30";
                else
                    txtPageSize.Text = value.ToString();
            }
        }
        /// <summary>
        /// 获取或设置分页的总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                int _pageCount = 0;
                int.TryParse(pageCount.Value, out _pageCount);
                return _pageCount;
            }
            set
            {
                if (value >= 0)
                    pageCount.Value = value.ToString();
                else
                    pageCount.Value = "0";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            OnFirstPager(PageSize, 1);
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            OnLastPager(PageSize, PageCount);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            OnNextPager(PageSize, PageIndex);
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            OnPrevPager(PageSize, PageIndex);
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            int _pageIndex = 0;
            if (int.TryParse(txtPage.Text, out _pageIndex))
            {
                if (_pageIndex > 0 && _pageIndex <= PageCount)
                    OnGotoPager(PageSize, _pageIndex);
                else
                    txtPage.Text = PageIndex.ToString();
            }
        }
    }
}