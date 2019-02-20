<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SoftwareDevelopmentTest._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentMain" runat="server">

    <asp:Label runat="server" CssClass="HeaderText">Summary:</asp:Label>
    <br />
    <asp:Label runat="server">
        This site is for you to use to test your knowledge in preperation for the Software Development Fundamentals MTA for C#.
        <br /><br />
        The questions given to you are <b>Random</b> from a pool of questions much like the MTA test and are from the course tests on the VLE.
    </asp:Label>
    <br /><br />
    <asp:Button ID="buttonStartTest" OnClick="buttonStartTest_Click" CssClass="HomeButton" runat="server" Text="Start Test" />
    
</asp:Content>
