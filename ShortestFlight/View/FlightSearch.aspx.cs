using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using ShortestFlight.Controller;
using ShortestFlight.Model;

namespace ShortestFlight
{
    public partial class FlightSearch : System.Web.UI.Page
    {
        AirportsController acontroller = new AirportsController();
        ResultsController rcontroller = new ResultsController();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                DropDownListFrom.DataSource = acontroller.getListCountries();
                DropDownListFrom.DataBind();
                DropDownListFrom.Items.Insert(0, "--Please select Country first--");
                DropDownListCity = new DropDownList();
                DropDownListCity.DataBind();
                DropDownListAirports = new DropDownList();
                DropDownListAirports.DataBind();
                DropDownListRFrom.DataSource = acontroller.getListCountries(); ;
                DropDownListRFrom.DataBind();
                DropDownListRFrom.Items.Insert(0, "--Please select Country first--");
                DropDownListRCity = new DropDownList();
                DropDownListRCity.DataBind();
                DropDownListRAirports = new DropDownList();
                DropDownListRAirports.DataBind();
            }
        }
        public List<Result> listResults { set; get; }
        public String errMessage { set; get; }
        protected void DropDownListCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListAirports.DataSource = acontroller.getListAirports(DropDownListCity.SelectedItem.Text);
            DropDownListAirports.DataBind();
            DropDownListAirports.Items.Insert(0, "--Please select Airport--");
        }
        protected void DropDownListFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListCity.DataSource = acontroller.getListCities(DropDownListFrom.SelectedItem.Text);
            DropDownListCity.DataBind();
            DropDownListCity.Items.Insert(0, "--Please select City--");
        }
        protected void DropDownListAirports_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownListRCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListRAirports.DataSource = acontroller.getListAirports(DropDownListRCity.SelectedItem.Text); ;
            DropDownListRAirports.DataBind();
            DropDownListRAirports.Items.Insert(0, "--Please select Airport--");
        }
        protected void DropDownListRFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListRCity.DataSource = acontroller.getListCities(DropDownListRFrom.SelectedItem.Text);
            DropDownListRCity.DataBind();
            DropDownListRCity.Items.Insert(0, "--Please select City--");
        }
        protected void DropDownListRAirports_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                listResults = rcontroller.getListResults(DropDownListAirports.SelectedItem.Text,
                 DropDownListRAirports.SelectedItem.Text);
                    RepeaterResults.DataSource = listResults;
                    RepeaterResults.DataBind();
          
             

            }
            catch (Exception exc)
            {
                if(Page.IsPostBack)
                    LiteralEroor.Text = "No result found!";
         

            }
        }
    }
}