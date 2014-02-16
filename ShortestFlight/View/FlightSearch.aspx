<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlightSearch.aspx.cs" Inherits="ShortestFlight.FlightSearch" %>

<%@ Import Namespace="System.Data" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link rel="stylesheet" type="text/css" href="~/Content/Site.css"/>
    <title>ShortestFlight</title>
  
</head>
<body>
    <div class="picture">


          <asp:Image ID="Image1" runat="server" ImageUrl="http://i99.photobucket.com/albums/l298/daniel_pg/plane.png?t=1392578744" /> 
    </div>

    <form id="searchForm" runat="server">

        <div class="Departure_From" style="margin-bottom: 30px">

            <p style="width: 200px; font-size: 20px; font-weight: bold">
                From
            </p>


            <asp:DropDownList ID="DropDownListFrom" runat="server" Height="26px"
                Width="222px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListFrom_SelectedIndexChanged">
            </asp:DropDownList>

            <asp:DropDownList ID="DropDownListCity" runat="server" AutoPostBack="True"
                Height="26px" OnSelectedIndexChanged="DropDownListCity_SelectedIndexChanged" Width="222px">
            </asp:DropDownList>

            <asp:DropDownList ID="DropDownListAirports" runat="server" AutoPostBack="True"
                Height="26px" OnSelectedIndexChanged="DropDownListAirports_SelectedIndexChanged" Width="222px">
            </asp:DropDownList>

        </div>


        <div class="Destination" style="margin-bottom: 30px">

            <p style="width: 200px; font-size: 20px; font-weight: bold">
                To
            </p>

            <asp:DropDownList ID="DropDownListRFrom" runat="server" Height="26px"
                Width="222px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListRFrom_SelectedIndexChanged">
            </asp:DropDownList>

            <asp:DropDownList ID="DropDownListRCity" runat="server" AutoPostBack="True"
                Height="26px" OnSelectedIndexChanged="DropDownListRCity_SelectedIndexChanged" Width="222px">
            </asp:DropDownList>

            <asp:DropDownList ID="DropDownListRAirports" runat="server" AutoPostBack="True"
                Height="26px" OnSelectedIndexChanged="DropDownListRAirports_SelectedIndexChanged" Width="222px">
            </asp:DropDownList>

        </div>

        <div>
            <asp:Button ID="ButtonSearch" runat="server" BackColor="#222123" ForeColor="White" Font-Bold="true"
                BorderColor="White" BorderStyle="Groove" Text="SEARCH FOR FLIGHTS"
                Width="170px" OnClick="ButtonSearch_Click" Height="33px" />
        </div>


        <div class="Results" style="margin-top: 20px">

            <asp:Repeater ID="RepeaterResults" runat="server">

                <ItemTemplate>
                    <table class="flat-table flat-table-1">

                        <tr style="font-weight: bold; font-size: 17px; color: black">

                            <td>

                                <%#Eval("fromAirport")  %>
                                <%#Eval ("toAirport") %>
                            </td>

                            <td><%#Eval("stops")  %> STOPS</td>

                            <td><%#Eval("distance")  %> km. </td>

                            <td></td>
                    </table>
                    <table>

                        <tr>
                            <td></td>

                            <td><%#(  String.Join("</br>", ((List<String>)Eval("FullRoute")).ToArray()))%> </td>
                        </tr>
                    </table>

                </ItemTemplate>
            </asp:Repeater>
        </div>

        <asp:Literal ID="LiteralEroor" runat="server"></asp:Literal>
    </form>
</body>
</html>
