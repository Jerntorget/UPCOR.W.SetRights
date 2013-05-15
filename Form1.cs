using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPCOR.W.SetRights
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        ClientContext _ctx;

        public Form1() {
            InitializeComponent();
            lblStatus.Text = string.Empty;
        }

        // ge alla
        private void button1_Click(object sender, EventArgs e) {
            string url = "http://login.tillsynen.se/borlange/";
            using (_ctx = new ClientContext(url)) {
                List listFors = _ctx.Web.Lists.GetByTitle("Försäljningsställen");
                ListItemCollection colItems = listFors.GetItems(CamlQuery.CreateAllItemsQuery());
                _ctx.Load(colItems);
                _ctx.ExecuteQuery();
                foreach (ListItem item in colItems) {
                    SetRights(item);
                }
                _ctx.ExecuteQuery();
            }
            lblStatus.Text = "Klar " + DateTime.Now.ToShortTimeString();
        }

        private void SetRights(ListItem item) {
            FieldLookupValue adress = (FieldLookupValue)item["Adress"]; ;
            FieldLookupValue agare = (FieldLookupValue)item["_x00c4_gare"]; ;
            FieldLookupValue[] kontakt = (FieldLookupValue[])item["Kontaktperson"];
            string kundnummer = (string)item["Kundnummer"];
            int id = item.Id;

            Group groupKund = CreateOrFindGroup(kundnummer);
            _ctx.Load(groupKund, g => g.Id);

            //RoleDefinition roleRead = _ctx.Web.RoleDefinitions.GetByType(RoleType.Reader);
            //GiveRightsToGroup(groupKund, roleRead, "Försäljningsställen", id, true);
            RoleDefinition roleEdit = _ctx.Web.RoleDefinitions.GetByType(RoleType.Editor);
            GiveRightsToGroup(groupKund, roleEdit, "Ägare", agare.LookupId, true);
            GiveRightsToGroup(groupKund, roleEdit, "Försäljningsställen Adresser", adress.LookupId, true);
            foreach (FieldLookupValue k in kontakt) {
                GiveRightsToGroup(groupKund, roleEdit, "Kontakter", k.LookupId, true);
            }
            //ConnectStore(id, groupKund.Id);
        }

        private void InheritRights(ListItem item) {
            FieldLookupValue adress = (FieldLookupValue)item["Adress"]; ;
            FieldLookupValue agare = (FieldLookupValue)item["_x00c4_gare"]; ;
            FieldLookupValue[] kontakt = (FieldLookupValue[])item["Kontaktperson"];
            string kundnummer = (string)item["Kundnummer"];
            int id = item.Id;

            InheritRights("Försäljningsställen", id);
            InheritRights("Ägare", agare.LookupId);
            InheritRights( "Försäljningsställen Adresser", adress.LookupId);
            foreach (FieldLookupValue k in kontakt) {
                InheritRights("Kontakter", k.LookupId);
            }
        }

        private void InheritRights(string listtitle, int listitemid) {
            List listOwners = _ctx.Web.Lists.GetByTitle(listtitle);
            ListItem li = listOwners.GetItemById(listitemid);
            li.ResetRoleInheritance();
            li.Update();
        }

        private void ConnectStore(int listitemid, int groupid) {
            List listConnect = _ctx.Web.Lists.GetByTitle("Grupper för försäljningsställen");
            CamlQuery cq = new CamlQuery();

            cq.ViewXml = @"<View>  
        <Query> 
            <Where><Eq><FieldRef Name='F_x00f6_rs_x00e4_ljningsst_x00e4' LookupId='True' /><Value Type='Lookup'>" + listitemid.ToString() + @"</Value></Eq></Where> 
        </Query> 
    </View>";
            ListItemCollection items = listConnect.GetItems(cq);
            _ctx.Load(items);
            _ctx.ExecuteQuery();
            if (items.Count == 0) {
                ListItem item = listConnect.AddItem(new ListItemCreationInformation { });
                item["F_x00f6_rs_x00e4_ljningsst_x00e4"] = listitemid;
                item["Grupp"] = groupid;
                item.Update();
            }
            else {
                foreach (ListItem item in items) {
                    item["Grupp"] = groupid;
                    item.Update();
                }
            }

        }

        private Group CreateOrFindGroup(string kundnummer) {
            _ctx.Load(_ctx.Web.SiteGroups, gs => gs.Include(g => g.Title));
            _ctx.ExecuteQuery();
            foreach (Group g in _ctx.Web.SiteGroups) {
                if (g.Title.ToLower() == kundnummer.ToLower())
                    return g;
            }
            return _ctx.Web.SiteGroups.Add(new GroupCreationInformation { Title = kundnummer });
        }

        private void GiveRightsToGroup(Group group, RoleDefinition role, string listtitle, int listitemid, bool removeVisitors) {
            List listOwners = _ctx.Web.Lists.GetByTitle(listtitle);
            ListItem li = listOwners.GetItemById(listitemid);
            li.BreakRoleInheritance(true, true);
            IEnumerable<RoleAssignment> assignments = _ctx.LoadQuery(li.RoleAssignments.Include(ra => ra.Member, ra => ra.RoleDefinitionBindings.Include(rd => rd.Name)));
            _ctx.ExecuteQuery();
            
            if (removeVisitors) {
                RoleAssignment raBesokare = null;
                foreach (var assign in assignments) {
                    string loginname = assign.Member.LoginName.ToLower();
                    if (loginname == "besökare på borlänge") {
                        raBesokare = assign;
                    }
                }
                if (raBesokare != null) {
                    raBesokare.DeleteObject();
                }
            }

            try {
                _ctx.Load(role);
                RoleDefinitionBindingCollection rdb = new RoleDefinitionBindingCollection(_ctx);
                rdb.Add(role);
                li.RoleAssignments.Add(group, rdb);
                li.Update();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefreshOne_Click(object sender, EventArgs e) {
            string url = "http://login.tillsynen.se/borlange/";
            using (_ctx = new ClientContext(url)) {
                List listFors = _ctx.Web.Lists.GetByTitle("Försäljningsställen");
                ListItemCollection colItems = listFors.GetItems(CamlQuery.CreateAllItemsQuery());
                _ctx.Load(colItems);
                _ctx.ExecuteQuery();
                SetRights(colItems[2]);
                //SetRights(colItems[1]);
                //SetRights(colItems[2]);

                _ctx.ExecuteQuery();
            }
            lblStatus.Text = "Klar " + DateTime.Now.ToShortTimeString();
        }

        private void btnInherit_Click(object sender, EventArgs e) {
            string url = "http://login.tillsynen.se/borlange/";
            using (_ctx = new ClientContext(url)) {
                List listFors = _ctx.Web.Lists.GetByTitle("Försäljningsställen");
                ListItemCollection colItems = listFors.GetItems(CamlQuery.CreateAllItemsQuery());
                _ctx.Load(colItems);
                _ctx.ExecuteQuery();
                //int i = 0;
                //string imax = colItems.Count.ToString();
                //int f = 0; int s = 0;
                //var ui = TaskScheduler.FromCurrentSynchronizationContext();
                //foreach (ListItem item in colItems) {
                //    i++;
                //    Task.Factory.StartNew(() => InheritRights(item)).ContinueWith(t => {
                //        if (t.IsFaulted) {
                //            f++;
                //        }
                //        else if (t.IsCompleted) {
                //            s++;
                //        }
                //        lblStatus.Text = "Started: " + i.ToString() + ", Succeeded: " + s.ToString() + ", Failed: " + f.ToString() + ", Max: " + imax;
                //    }, ui);
                //}
                foreach (ListItem item in colItems) {
                    InheritRights(item);
                }
                _ctx.ExecuteQuery();
                lblStatus.Text = "Klar " + DateTime.Now.ToShortTimeString();
            }
        }
    }
}
