using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.FMSystem.BLL;
using FM.TransportMaintenanceDLL.DAL;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using FM.FMSystem.DAL;
using TR_LanguageResource.Resources;

namespace FM.TransportMaintenanceDLL.BLL
{
    public class DriverIncentive
    {
        private string incentiveReference;
        private DateTime validFrom;
        private DateTime validTo;
        private SortableList<DriverIncentiveDetail> driverIncentiveDetails;

        public DriverIncentive()
        {
            this.incentiveReference = "";
            this.validFrom=DateTime.Today;
            this.validTo=DateTime.Today;
            this.driverIncentiveDetails=new SortableList<DriverIncentiveDetail>();

        }

        public DriverIncentive(string incentiveReference, DateTime validFrom,
            DateTime validTo, SortableList<DriverIncentiveDetail> driverIncentiveDetails)
        {
            this.IncentiveReference = incentiveReference;
            this.ValidFrom = validFrom;
            this.ValidTo = validTo;
            this.DriverIncentiveDetails = driverIncentiveDetails;

        }

        public string IncentiveReference
        {
            get { return incentiveReference; }
            set { incentiveReference = value; }
        }

        public DateTime ValidFrom
        {
            get { return validFrom; }
            set { validFrom = value; }
        }

        public DateTime ValidTo
        {
            get { return validTo; }
            set { validTo = value; }
        }

        public SortableList<DriverIncentiveDetail> DriverIncentiveDetails
        {
            get { return driverIncentiveDetails; }
            set { driverIncentiveDetails = value; }
        }



        public bool AddDriverIncentive()
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction(); 
            try
            {
                if (ValidateAddDriverIncentive() == true)
                {
                    DriverIncentiveDAL.AddDriverIncentive(this, con, tran);
                    DateTime serverDateTime = Logger.GetServerDateTime();

                    LogHeader logHeader = new LogHeader(FMModule.Transport, "DRIVER INCENTIVE", serverDateTime,
                        incentiveReference, incentiveReference, FormMode.Add, "ipl");

                    LogDetail logDetail1 = new LogDetail("incentiveReference", incentiveReference);
                    LogDetail logDetail2 = new LogDetail("validTo", validTo.ToString());

                    logHeader.LogDetails.Add(logDetail1);
                    logHeader.LogDetails.Add(logDetail2);

                    Logger.WriteLog(logHeader, con, tran);

                    tran.Commit();
                }
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw new FMException(TptResourceDAL.ErrInsertFailed + " Driver Incentives. \n" + ex.ToString());
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(TptResourceDAL.ErrInsertFailed + " Driver Incentives. \n" + ex.ToString());
            }
            finally
            {
                con.Close();
            } 
            return true;

        }

        public bool EditDriverIncentive()
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (validFrom <= validTo)
                {
                    DriverIncentiveDAL.EditDriverIncentive(this, con, tran);
                    DateTime serverDateTime = Logger.GetServerDateTime();

                    LogHeader logHeader = new LogHeader(FMModule.Transport, "DRIVER INCENTIVE", serverDateTime,
                        incentiveReference, incentiveReference, FormMode.Add, "ipl");

                    LogDetail logDetail1 = new LogDetail("incentiveReference", incentiveReference);
                    LogDetail logDetail2 = new LogDetail("validTo", validTo.ToString());

                    logHeader.LogDetails.Add(logDetail1);
                    logHeader.LogDetails.Add(logDetail2);

                    Logger.WriteLog(logHeader, con, tran);

                    tran.Commit();
                }
                else
                    throw new FMException(TptResourceBLL.ErrValidFromLaterTanValidTo);
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(TptResourceDAL.ErrEditFailed + " Driver Incentives. \n" + ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return true;

        }


        public bool ValidateAddDriverIncentive()
        {
            string error = "";
            bool status = true;
            //May 21, 2012 - Gerry modified
            //if (String.Compare(validFrom.ToShortDateString(), validTo.ToShortDateString()) > 0)
            //{
            //    error += TptResourceBLL.ErrValidFromLaterTanValidTo;
            //    status = false;
            //}
            if (DateTime.Compare(validFrom, validTo) > 0)
            {
                status = false;
                throw new FMException(TptResourceBLL.ErrValidFromLaterTanValidTo);
                
            }
            if (DriverIncentiveDAL.VerifyValidDate(validFrom) == false)
            {
                status = false;
                throw new FMException(TptResourceBLL.ErrInvalidPeriod);
            }
            return status;
        }

        public bool AddDriverIncentiveDetail(DriverIncentiveDetail d, string userid)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                    DriverIncentiveDAL.AddDriverIncentiveDetail(this, d,con, tran);
                    DateTime serverDateTime = Logger.GetServerDateTime();

                    LogHeader logHeader = new LogHeader(FMModule.Transport, "DRIVER INCENTIVE", serverDateTime,
                        incentiveReference, incentiveReference, FormMode.Add, userid);

                    LogDetail logDetail1 = new LogDetail("incentiveReference", incentiveReference);
                    LogDetail logDetail2 = new LogDetail("incentivepertrip", d.IncentivePerTrip.ToString());

                    logHeader.LogDetails.Add(logDetail1);
                    logHeader.LogDetails.Add(logDetail2);

                    Logger.WriteLog(logHeader, con, tran);

                    tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return true;

        }

        public bool EditDriverIncentiveDetail(DriverIncentiveDetail d, string userid)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                DriverIncentiveDAL.EditDriverIncentiveDetail(this, d, con, tran);
                DateTime serverDateTime = Logger.GetServerDateTime();

                LogHeader logHeader = new LogHeader(FMModule.Transport, "DRIVER INCENTIVE", serverDateTime,
                    incentiveReference, incentiveReference, FormMode.Add, userid);

                LogDetail logDetail1 = new LogDetail("incentiveReference", incentiveReference);
                LogDetail logDetail2 = new LogDetail("incentivepertrip", d.IncentivePerTrip.ToString());

                logHeader.LogDetails.Add(logDetail1);
                logHeader.LogDetails.Add(logDetail2);

                Logger.WriteLog(logHeader, con, tran);

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return true;

        }


        public bool DeleteDriverIncentiveDetail(DriverIncentiveDetail d)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                DriverIncentiveDAL.DeleteDriverIncentiveDetail(this, d, con, tran);

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return true;

        }

        public static SortableList<DriverIncentive> GetAllDriverIncentive()
        {
            return DriverIncentiveDAL.GetAllDriverIncentives();
        }


        public void GetDriverIncentiveDetail()
        {
            this.driverIncentiveDetails = DriverIncentiveDAL.GetDriverIncentiveDetail(this);

        }

        public void CopyDriverIncentive(string fromdriverRef,string userid)
        {
            try
            {
                SortableList<DriverIncentiveDetail> temp = new SortableList<DriverIncentiveDetail>();
                temp = DriverIncentiveDAL.GetDriverIncentiveDetail(fromdriverRef);
                for (int i = 0; i < temp.Count; i++)
                {
                    this.AddDriverIncentiveDetail(temp[i], userid);
                    this.driverIncentiveDetails.Add(temp[i]);
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


    }
}
