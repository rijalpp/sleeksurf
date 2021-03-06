﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.Web.Admin.UserControls.Interfaces;
using SleekSurf.Web.Admin.UserControls.Presenters;

namespace SleekSurf.Web.Admin.UserControls
{
    public partial class SANavigationMenu : System.Web.UI.UserControl, ISANavigationMenu
    {
        private SANavigationMenuPresenter presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new SANavigationMenuPresenter();
            presenter.Initialize(this);

            if (!IsPostBack)
            {
                presenter.LoadMenuItems(Menu);
            }
        }

        protected void Menu_MenuItemClick(object sender, MenuEventArgs e)
        {
            presenter.MaintainFamilyCycle();

            presenter.RedirectToCorrespondingPage(e.Item.Text);
        }
    }
}