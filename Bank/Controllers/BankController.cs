using Bank.Data;
using Bank.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Bank.Controllers
{
    public class BankController : Controller
    {
        // GET: Bank

        private BankEntities db = new BankEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("Login");
        }
        
        [HttpPost]
        public ActionResult Login(LogViewModel model)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = new SqlConnection(@"Server=SUYPC210\SQLEXPRESS;Database=Bank;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from loginTable where username = '" + model.username + "' and password = '" + model.password + "'", con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    model.username = reader["username"].ToString();
                    model.password = reader["password"].ToString();
                    int loginid = (int)reader["logid"];

                    reader.Close();
                    con.Close();
                    con.Open();
                    /*-----------------------------*/
                    //SqlCommand cmd1 = new SqlCommand("select * from userTable where lid = '" + loginid + "'", con);
                    //SqlDataReader reader1 = cmd.ExecuteReader();
                    //if (reader1.Read())
                    //{
                    //    Session["currentAcc"] = (int)reader1["accountnumber"];
                    //}
                    /*-----------------------------*/
                    FormsAuthentication.SetAuthCookie(model.username, true);
                    return View("UserHome", model);
                }
                else
                {
                    ModelState.AddModelError("Error", "Incorrect Credentials");
                    TempData["AlertMessage"] = "Username or Password Incorrect !!!";
                    return View("Login", model);
                }
                reader.Close();
                con.Close();
            }
            else
            {
                return View("Login", model);
            }
            
            
        }


        public ActionResult TransferMoney()
        {
            return View("TransferMoney");
        }

        [HttpPost]
        public ActionResult TransferMoney(TransferViewModel model)
        {
            SqlConnection con = new SqlConnection(@"Server=SUYPC210\SQLEXPRESS;Database=Bank;Integrated Security=True");
            con.Open();
            SqlTransaction transaction = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
           
            if (ModelState.IsValid)
            {
                int senderAccountNumber = (int)model.SenderAccountNumber;
                int receiverAccountNumber = (int)model.ReceiverAccountNumber;
                decimal amount =  model.TransferAmount;
                decimal SavailableBalance = 0;
                decimal ScurrentBalance = 0;
                try
                {
                    string sql = "select balanceamount from accountTable where accountnumber = '" + senderAccountNumber + "'";
                    SqlCommand cmd = new SqlCommand(sql, con,transaction);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                            SavailableBalance = Convert.ToDecimal(reader["balanceamount"]);
                            ScurrentBalance = SavailableBalance - amount;
                    }
                    reader.Close();

                    /*---------------------------------------------*/

                    decimal RavailableBalance = 0;
                    decimal RcurrentBalance = 0;
                    string sql1 = "select balanceamount from accountTable where accountnumber = '" + receiverAccountNumber + "'";
                    SqlCommand cmd1 = new SqlCommand(sql1, con, transaction);
                    SqlDataReader reader1 = cmd1.ExecuteReader();
                    while (reader1.Read())
                    {
                        RavailableBalance = Convert.ToDecimal(reader1["balanceamount"]);
                        RcurrentBalance = RavailableBalance + amount;

                    }
                    reader1.Close();

                    /*-------------------------------------------*/

                    string sql2 = "update accountTable set balanceamount = '" + ScurrentBalance + "' where accountnumber = '" + senderAccountNumber + "'";
                    SqlCommand cmd2 = new SqlCommand(sql2, con, transaction);
                    cmd2.ExecuteNonQuery();
                    string sql3 = "insert into transactionTable (accountnumber, debit, remark) values ('" + senderAccountNumber + "', '" + amount + "', 'DEBIT') ";
                    SqlCommand cmd3 = new SqlCommand(sql3, con, transaction);
                    cmd3.ExecuteNonQuery();
                   
                   /*------------------------------------------------------------------------------------------------------------------*/
                    
                    string sql5 = "update accountTable set balanceamount = '" + RcurrentBalance + "' where accountnumber = '" + receiverAccountNumber + "'";
                    SqlCommand cmd5 = new SqlCommand(sql5, con, transaction);
                    cmd5.ExecuteNonQuery();
                    string sql4 = "insert into transactionTable (accountnumber, credit, remark) values ('" + receiverAccountNumber + "', '" + amount + "', 'CREDIT') ";
                    SqlCommand cmd4 = new SqlCommand(sql4, con, transaction);
                    cmd4.ExecuteNonQuery();
                    
                    /*---------------------------------------------------------------------*/

                    transaction.Commit();

                    con.Close();
                }
                catch (Exception ex)
                {
                    //ex.StackTrace();
                    transaction.Rollback();
                    TempData["AlertMessage"] = "Transaction Aborted !!!";
                    return View("TransferMoney", model);
                }
                return RedirectToAction("Index");
            }
            else
            {
               
                TempData["AlertMessage"] = "Error Occured !!!";
                return View("TransferMoney", model);
            }

           
        }


        public ActionResult Recharge()
        {
            return View("Recharge");
        }

        [HttpPost]
        public ActionResult Recharge(RechargeViewModel model)
        {
            SqlConnection con = new SqlConnection(@"Server=SUYPC210\SQLEXPRESS;Database=Bank;Integrated Security=True");
            con.Open();
            SqlTransaction transactions = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            if (ModelState.IsValid)
            {
                int accountNumber = model.accountnumber;
                decimal rechargeAmount = model.amount;
                string mobile = model.mobileNo;
                decimal availableBalance = 0;
                decimal currentBalance = 0;
                try
                {

                    string sql = "select balanceamount from accountTable where accountnumber = '" + accountNumber + "'";
                    SqlCommand cmd = new SqlCommand(sql, con, transactions);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        availableBalance = Convert.ToDecimal(reader["balanceamount"]);
                        currentBalance = availableBalance - rechargeAmount;
                    }
                    reader.Close();


                    string sql2 = "update accountTable set balanceamount = '" + currentBalance + "' where accountnumber = '" + accountNumber + "'";
                    SqlCommand cmd2 = new SqlCommand(sql2, con, transactions);
                    cmd2.ExecuteNonQuery();
                    string sql3 = "insert into transactionTable (accountnumber, debit, remark) values ('" + accountNumber + "', '" + rechargeAmount + "', 'RECHARGE') ";
                    SqlCommand cmd3 = new SqlCommand(sql3, con, transactions);
                    cmd3.ExecuteNonQuery();

                    transactions.Commit();

                    con.Close();

                }
                catch (Exception ex)
                {
                    transactions.Rollback();
                    TempData["AlertMessage"] = "Transaction Aborted !!!";
                    return View("Recharge", model);
                }
                return RedirectToAction("Index");
            }
            else
            {
                TempData["AlertMessage"] = "Error Occured !!!";
                return View("Recharge", model);
            }
        }



        public ActionResult LastTransaction()
        {
          
            return View("");
        }


        public ActionResult LastTransactionsView(LastViewModel model)
        {
            if (ModelState.IsValid)
            {
              

                var entities = new BankEntities();
                var query1 = from c in (from e in entities.transactionTables where e.accountnumber == model.accountNumber select e).OrderByDescending(x => x.transid).Take(5) select c ;
                

                return View(query1.ToList());

            }
            return View("");

        }


        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}