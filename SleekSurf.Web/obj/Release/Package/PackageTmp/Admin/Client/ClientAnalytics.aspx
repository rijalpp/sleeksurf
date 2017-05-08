<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientAnalytics.aspx.cs"
    Inherits="SleekSurf.Web.Admin.Client.ClientAnalytics" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/LoginDialog/jquery-1.4.3.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $("#HitLocation>div").click(function () {
                $(this).children(".locationsCity").slideToggle("slow");
                if ($(this).children(".expandCollapse").is(".collapse")) {
                    $(this).children(".expandCollapse").removeClass("collapse").addClass("expand");
                }
                else {
                    $(this).children(".expandCollapse").removeClass("expand").addClass("collapse");
                }
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="HitLocationWrapper">
        <p class="locationDescription">
            <asp:Label ID="lblLocationDescription" runat="server">
            </asp:Label>
        </p>
        <div id="HitLocation" runat="server">
            <asp:Repeater ID="rptrLocation" runat="server" OnItemDataBound="rptrLocation_ItemDataBound">
                <HeaderTemplate>
                    <div>
                        <span class="location">Location</span> <span class="visit">Visits</span>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="alternateRow">
                        <span class="expandCollapse"></span><span class="location">
                            <%# Eval("CountryName") %></span> <span class="visit">
                                <%# Eval("NumberOfVisits")%></span>
                        <asp:HiddenField ID="hfCountry" Value='<%# Eval("CountryName") %>' runat="server" />
                        <div class="locationsCity">
                            <asp:Repeater ID="rptrLocationsCity" runat="server">
                                <ItemTemplate>
                                    <div>
                                        <span>
                                            <%# Eval("CityName")%></span> <span class="visit">
                                                <%# Eval("NumberOfVisits")%></span>
                                    </div>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <div>
                                        <span>
                                            <%# Eval("CityName")%></span> <span class="visit">
                                                <%# Eval("NumberOfVisits")%></span>
                                    </div>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <div>
                        <span class="expandCollapse"></span><span class="location">
                            <%# Eval("CountryName") %></span> <span class="visit">
                                <%# Eval("NumberOfVisits")%></span>
                        <asp:HiddenField ID="hfCountry" Value='<%# Eval("CountryName") %>' runat="server" />
                        <div class="locationsCity">
                            <asp:Repeater ID="rptrLocationsCity" runat="server">
                                <ItemTemplate>
                                    <div>
                                        <span>
                                            <%# Eval("CityName")%></span> <span class="visit">
                                                <%# Eval("NumberOfVisits")%></span>
                                    </div>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <div>
                                        <span>
                                            <%# Eval("CityName")%></span> <span class="visit">
                                                <%# Eval("NumberOfVisits")%></span>
                                    </div>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    </form>
</body>
</html>
