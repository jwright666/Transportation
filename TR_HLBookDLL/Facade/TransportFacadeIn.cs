using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_HLBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_SeaFreightDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_SeaFreightDLL.Facade;

namespace FM.TR_HLBookDLL.Facade
{
    public class TransportFacadeIn
    {
        public static List<ContainerTypesDTO> GetAllContainerTypes()
        {
            return SeaFreightFacadeOut.GetAllContainerTypes();
        }

        public static List<CustomerDTO> GetAllCustomers()
        {
            return CustomerDTO.GetAllCustomers();
        }

        public static List<OperatorDTO> GetAllOperators()
        {
            //20141031 - gerry modified to insert blank operation 
            List<OperatorDTO> operators = OperatorDTO.GetAllOperators();
            operators.Insert(0, new OperatorDTO());
            return operators;
        }

        public static List<BranchDTO> GetAllBranches()
        {
            return BranchDTO.GetAllBranches();
        }

        public static List<string> GetOutstandingSeaExportJobNos(string transporterCode)
        {
            return SeaFreightFacadeOut.GetOutstandingSeaExportJobNos(transporterCode);
        }

        public static List<string> GetOutstandingSeaImportJobNos(string transporterCode)
        {
            return SeaFreightFacadeOut.GetOutstandingSeaImportJobNos(transporterCode);
        }

        public static SeaExportJob GetSeaExportJob(string jobNo)
        {
            return SeaFreightFacadeOut.GetSeaExportJob(jobNo);
        }

        public static SeaImportJob GetSeaImportJob(string jobNo)
        {
            return SeaFreightFacadeOut.GetSeaImportJob(jobNo);

        }

        // 2014-07-01 Zhou Kai modifies this function, for new fields from base class SeaJob:
        // emptyContainerToXXX, cargoDescription, shipping_line_code
        public static HaulierJob ConvertSeaImportDataToHaulierJob(SeaImportJob seaImportJob)
        {
            HaulierJob haulierJob = new HaulierJob();

            #region "2014-09-01 Zhou Kai modifies this block, the customer code is now not default to shipper"
            //OperationDetail operationDetail = new OperationDetail();
            //// operationDetail = OperationDetail.GetOperationDetail(seaImportJob.ConsigneeCode);
            //operationDetail = OperationDetail.GetOperationDetail(seaImportJob.CustomerCode);
            //if (operationDetail == null || operationDetail.customerCode == "") 
            //{
            //    haulierJob.CustNo = "CASH";
            //}
            //{
            //    haulierJob.CustNo = operationDetail.customerCode;
            //}            
            #endregion

            //20140913 -  gerry modified use customerDTO instead of operationDetail because sometime customer are not in opertaionDetail
            CustomerDTO cust = CustomerDTO.GetCustomerByCustCode(seaImportJob.CustomerCode);
            if (cust != null)
                haulierJob.CustNo = cust.Code;
            else
                haulierJob.CustNo = "CASH";


            #region "2014-07-01 Zhou Kai adds"

            haulierJob.ShippingLine = seaImportJob.Shipping_Line_Code;
            haulierJob.ShippingLineName = seaImportJob.Shipping_Line_Name;
            haulierJob.CargoDescription = seaImportJob.CargoDescription;
            haulierJob.CustomerRef = seaImportJob.CustomerRefNo; // 2014-07-24 Zhou Kai adds

            //20150803 - gerry modified for container yard
            haulierJob.ContainerYard = new Stop(
                seaImportJob.DeliveryCode,
                seaImportJob.DeliveryName,
                seaImportJob.DeliveryAdd1,
                seaImportJob.DeliveryAdd2,
                seaImportJob.DeliveryAdd3,
                seaImportJob.DeliveryAdd4,
                seaImportJob.DeliveryCityCode
                );
            //20150803 - gerry modified for Sea Import job the pickup place is the Port
            haulierJob.UnstuffingAt = new Stop(
                seaImportJob.StuffingCode,
                seaImportJob.StuffingName,
                seaImportJob.StuffingAdd1,
                seaImportJob.StuffingAdd2,
                seaImportJob.StuffingAdd3,
                seaImportJob.StuffingAdd4,
                seaImportJob.StuffingCityCode
            #region Removed
                //seaImportJob.PickUpPlaceCode,
                //seaImportJob.PickUpPlaceName,
                //seaImportJob.PickUpPlaceAdd1,
                //seaImportJob.PickUpPlaceAdd2,
                //seaImportJob.PickUpPlaceAdd3,
                //seaImportJob.PickUpPlaceAdd4,
                //seaImportJob.PickUpPlaceCityCode
            #endregion
                );
            //20150803 - gerry modified for Sea Import job the pickup place is the Port
            haulierJob.Port = new Stop(
                seaImportJob.PickUpPlaceCode,
                seaImportJob.PickUpPlaceName,
                seaImportJob.PickUpPlaceAdd1,
                seaImportJob.PickUpPlaceAdd2,
                seaImportJob.PickUpPlaceAdd3,
                seaImportJob.PickUpPlaceAdd4,
                seaImportJob.PickUpPlaceCityCode
                );
            // default to default code in operating database
            //OperatorDTO Port = OperatorDTO.GetOperatorDTO(TransportSettings.GetTransportSetting().DefaultPortCode);
            //haulierJob.Port = new Stop(Port.Code, Port.Name, Port.Add1, Port.Add2, Port.Add3, Port.Add4, Port.City);
            // 2014-07-10 Zhou Kai comments out below:
            //haulierJob.StuffingAt = new Stop(
            //    dictPort["Operation_Code"].ToString(),
            //    dictPort["Operation_Name"].ToString(),
            //    dictPort["Op_Add1"].ToString(),
            //    dictPort["Op_Add2"].ToString(),
            //    dictPort["Op_Add3"].ToString(),
            //    dictPort["Op_Add4"].ToString(),
            //    dictPort["City_Code"].ToString()
            //    );
            // 2014-07-10 Zhou Kai ends
            haulierJob.BookingRef = seaImportJob.BookingRefNo;
            #endregion

            haulierJob.SourceReferenceNo = seaImportJob.JobNumber;
            haulierJob.JobStatus = JobStatus.Booked;
            haulierJob.JobType = HaulierJob.HAULIER_SEA_IMPORT;
            haulierJob.JobDateTime = DateTime.Today;
            haulierJob.BranchCode = seaImportJob.BranchCode;
            haulierJob.JobUrgencyType = JobUrgencyType.Normal;

            haulierJob.shipperCode = seaImportJob.ShipperCode;
            haulierJob.shipperName = seaImportJob.ShipperName;
            haulierJob.shipperAdd1 = seaImportJob.ShipperAdd1;
            haulierJob.shipperAdd2 = seaImportJob.ShipperAdd2;
            haulierJob.shipperAdd3 = seaImportJob.ShipperAdd3;
            haulierJob.shipperAdd4 = seaImportJob.ShipperAdd4;

            haulierJob.consigneeCode = seaImportJob.ConsigneeCode;
            haulierJob.consigneeName = seaImportJob.ConsigneeName;
            haulierJob.consigneeAdd1 = seaImportJob.ConsigneeAdd1;
            haulierJob.consigneeAdd2 = seaImportJob.ConsigneeAdd2;
            haulierJob.consigneeAdd3 = seaImportJob.ConsigneeAdd3;
            haulierJob.consigneeAdd4 = seaImportJob.ConsigneeAdd4;

            haulierJob.oblNo = seaImportJob.OblNumber;
            haulierJob.hblNo = seaImportJob.HblNumber;
            haulierJob.VesselNo = seaImportJob.MVessel;
            haulierJob.VesselName = seaImportJob.MVesselName; // 2014-07-11 Zhou Kai adds
            haulierJob.voyageNo = seaImportJob.MVoyage;
            haulierJob.POL = seaImportJob.Pol;
            haulierJob.PolName = seaImportJob.POLName;
            haulierJob.PodName = seaImportJob.PODName;
            haulierJob.POD = seaImportJob.Pod;
            haulierJob.eta = seaImportJob.Eta;
            haulierJob.etd = seaImportJob.Etd;

            return haulierJob;
        }

        // 2014-07-01 Zhou Kai modifies this function, for the new fields:
        // emptyContainerToXXX, cargoDescription, shipping_line_code from base class SeaExportJob
        // bookingRefNo, newly added to SeaExportJob only
        public static HaulierJob ConvertSeaExportDataToHaulierJob(SeaExportJob seaExportJob)
        {
            HaulierJob haulierJob = new HaulierJob();
            #region "2014-09-01 Zhou Kai modifies this block, the customer code is now not default to shipper"
            //OperationDetail operationDetails = new OperationDetail();
            //// operationDetails = OperationDetail.GetOperationDetail(seaExportJob.ShipperCode);
            //operationDetails = OperationDetail.GetOperationDetail(seaExportJob.CustomerCode);
            //if (operationDetails == null || 
            //    operationDetails.customerCode.Equals(String.Empty)) // no such customer, ZK
            //{
            //    haulierJob.CustNo = "CASH"; 
            //}
            //else
            //{
            //    haulierJob.CustNo = operationDetails.customerCode; // get customer code, ZK
            //}

            #endregion

            //20140913 -  gerry modified use customerDTO instead of operationDetail because sometime customer are not in opertaionDetail
            CustomerDTO cust = CustomerDTO.GetCustomerByCustCode(seaExportJob.CustomerCode);
            if (cust != null)
                haulierJob.CustNo = cust.Code;
            else
                haulierJob.CustNo = "CASH";
            //20140913 end 
            #region "2014-07-01 Zhou Kai adds"
            haulierJob.ShippingLine = seaExportJob.Shipping_Line_Code;
            haulierJob.ShippingLineName = seaExportJob.Shipping_Line_Name;
            haulierJob.CargoDescription = seaExportJob.CargoDescription;
            haulierJob.BookingRef = seaExportJob.BookingRefNo;
            haulierJob.CustomerRef = seaExportJob.CustomerRefNo; // 2014-07-24 Zhou Kai adds
             
            //20150803 - gerry modified for container yard
            haulierJob.ContainerYard = new Stop(seaExportJob.PickUpPlaceCode,
                seaExportJob.PickUpPlaceName,
                seaExportJob.PickUpPlaceAdd1,
                seaExportJob.PickUpPlaceAdd2,
                seaExportJob.PickUpPlaceAdd3,
                seaExportJob.PickUpPlaceAdd4,
                seaExportJob.PickUpPlaceCityCode
                );

            //20150803 - gerry modified for Sea Export job the pickup place is the Port
            haulierJob.StuffingAt = new Stop(seaExportJob.StuffingCode,
                seaExportJob.StuffingName,
                seaExportJob.StuffingAdd1,
                seaExportJob.StuffingAdd2,
                seaExportJob.StuffingAdd3,
                seaExportJob.StuffingAdd4,
                seaExportJob.StuffingCityCode
                );
           
            //20150803 - gerry modified for Sea Import job the pickup place is the Port
            haulierJob.Port = new Stop(seaExportJob.DeliveryCode,
                seaExportJob.DeliveryName,
                seaExportJob.DeliveryAdd1,
                seaExportJob.DeliveryAdd2,
                seaExportJob.DeliveryAdd3,
                seaExportJob.DeliveryAdd4,
                seaExportJob.DeliveryCityCode
                );

            // 2014-07-10 Zhou Kai comments out below
            //haulierJob.UnstuffingAt = new Stop(seaExportJob.DeliveryToCode,
            //    seaExportJob.DeliveryToName, 
            //    seaExportJob.DeliveryToAdd1,
            //    seaExportJob.DeliveryToAdd2,
            //    seaExportJob.DeliveryToAdd3,
            //    seaExportJob.DeliveryToAdd4,
            //    seaExportJob.DeliveryToCityCode
            //    );
            // 2014-07-10 Zhou Kai ends
            #endregion

            haulierJob.SourceReferenceNo = seaExportJob.JobNumber;
            haulierJob.JobStatus = JobStatus.Booked;
            haulierJob.JobType = HaulierJob.HAULIER_SEA_EXPORT;
            haulierJob.JobDateTime = DateTime.Today;
            haulierJob.BranchCode = seaExportJob.BranchCode;
            haulierJob.JobUrgencyType = JobUrgencyType.Normal;

            haulierJob.shipperCode = seaExportJob.ShipperCode;
            haulierJob.shipperName = seaExportJob.ShipperName;
            haulierJob.shipperAdd1 = seaExportJob.ShipperAdd1;
            haulierJob.shipperAdd2 = seaExportJob.ShipperAdd2;
            haulierJob.shipperAdd3 = seaExportJob.ShipperAdd3;
            haulierJob.shipperAdd4 = seaExportJob.ShipperAdd4;

            haulierJob.consigneeCode = seaExportJob.ConsigneeCode;
            haulierJob.consigneeName = seaExportJob.ConsigneeName;
            haulierJob.consigneeAdd1 = seaExportJob.ConsigneeAdd1;
            haulierJob.consigneeAdd2 = seaExportJob.ConsigneeAdd2;
            haulierJob.consigneeAdd3 = seaExportJob.ConsigneeAdd3;
            haulierJob.consigneeAdd4 = seaExportJob.ConsigneeAdd4;

            haulierJob.oblNo = seaExportJob.OblNumber;
            haulierJob.hblNo = seaExportJob.HblNumber;
            haulierJob.VesselNo = seaExportJob.MVessel;
            haulierJob.VesselName = seaExportJob.MVesselName; // 2014-07-11 Zhou Kai adds
            haulierJob.voyageNo = seaExportJob.MVoyage;
            haulierJob.POL = seaExportJob.Pol;
            haulierJob.PolName = seaExportJob.POLName; // 2014-07-04 Zhou Kai adds
            haulierJob.POD = seaExportJob.Pod;
            haulierJob.PodName = seaExportJob.PODName; // 2014-07-04 Zhou Kai adds
            haulierJob.eta = seaExportJob.Eta;
            haulierJob.etd = seaExportJob.Etd;

            return haulierJob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seaExportJob"></param>
        /// <param name="haulierJob"></param>
        /// <param name="legType"></param>
        /// <returns></returns>
        public static SortableList<HaulierJobTrip> ConvertSeaExportContainerDataToHaulierTrip(SeaExportJob seaExportJob,
            HaulierJob haulierJob, Leg_Type legType)
        {
            SortableList<HaulierJobTrip> haulierJobTrips = new SortableList<HaulierJobTrip>();
            int nLegGroup = 1;

            if (legType == Leg_Type.OneLeg) // the job has only one leg, and every container corresponds to a leg
            {
                for (int i = 0; i < seaExportJob.SeaJobContainers.Count; i++)
                {
                    //20150825 - gery added checking if container has gross weight
                    if (seaExportJob.SeaJobContainers[i].GrossWeight <= 0) { throw new FMException("Gross weight should be greater than 0. "); }

                    HaulierJobTrip oneLeg = new HaulierJobTrip();
                    oneLeg.JobID = haulierJob.JobID;
                    oneLeg.LegGroup = nLegGroup++; // per container per leg_group
                    oneLeg.Sequence = i + 1;
                    oneLeg.TripStatus = JobTripStatus.Booked;
                    oneLeg.IsNew = true;

                    #region "start / end stop"
                    oneLeg.StartStop.Code = seaExportJob.PickUpPlaceCode;
                    oneLeg.StartStop.Description = seaExportJob.PickUpPlaceName;
                    oneLeg.StartStop.Address1 = seaExportJob.PickUpPlaceAdd1;
                    oneLeg.StartStop.Address2 = seaExportJob.PickUpPlaceAdd2;
                    oneLeg.StartStop.Address3 = seaExportJob.PickUpPlaceAdd3;
                    oneLeg.StartStop.Address4 = seaExportJob.PickUpPlaceAdd4;
                    oneLeg.EndStop.Code = seaExportJob.DeliveryCode;
                    oneLeg.EndStop.Description = seaExportJob.DeliveryName;
                    oneLeg.EndStop.Address1 = seaExportJob.DeliveryAdd1;
                    oneLeg.EndStop.Address2 = seaExportJob.DeliveryAdd2;
                    oneLeg.EndStop.Address3 = seaExportJob.DeliveryAdd3;
                    oneLeg.EndStop.Address4 = seaExportJob.DeliveryAdd4;
                    #endregion

                    #region "start / end time"
                    oneLeg.StartDate = DateTime.Today;
                    oneLeg.EndDate = DateTime.Today;
                    oneLeg.StartTime = "0000";
                    oneLeg.EndTime = "0000";
                    #endregion

                    OperationDetail operationDetail = new OperationDetail();
                    operationDetail = OperationDetail.GetOperationDetail(seaExportJob.ShipperCode);
                    CustomerDTO cust = new CustomerDTO();
                    if (operationDetail == null || operationDetail.customerCode == "")
                    {
                        cust = CustomerDTO.GetCustomerByCustCode("CASH");
                    }
                    else
                    {
                        cust = CustomerDTO.GetCustomerByCustCode(operationDetail.customerCode);
                    }

                    oneLeg.CustomerCode = cust.Code;
                    oneLeg.CustomerName = cust.Name;
                    oneLeg.OwnTransport = true;
                    //h1.SubContractorCode = "";   //20140312 - gerry replaced  
                    //201040502 - gerry change subContractorCode from CustomerDTO to OperatorDTO
                    oneLeg.subCon = new OperatorDTO();//new CustomerDTO();

                    oneLeg.SubContractorReference = "";
                    oneLeg.ChargeCode = "";
                    oneLeg.Remarks = "";

                    oneLeg.ContainerCode = seaExportJob.SeaJobContainers[i].ContainerCode;
                    ContainerTypesDTO containerTypesDTO = new ContainerTypesDTO();
                    oneLeg.ContainerNo = seaExportJob.SeaJobContainers[i].ContainerNumber;
                    oneLeg.SealNo = seaExportJob.SeaJobContainers[i].SealNumber;
                    oneLeg.GrossWeight = seaExportJob.SeaJobContainers[i].GrossWeight;
                    oneLeg.GrossCBM = seaExportJob.SeaJobContainers[i].GrossCBM == 0 ? containerTypesDTO.TareCBM : seaExportJob.SeaJobContainers[i].GrossCBM;
                    //firstLeg.GrossCBM = seaExportJob.SeaJobContainers[i].GrossCBM;
                    // firstLeg.ContainerCode = seaExportJob.SeaJobContainers[i].ContainerCode;
                    containerTypesDTO = ContainerTypesDTO.GetContainerDTO(oneLeg.ContainerCode);
                    oneLeg.maxCBM = oneLeg.GrossCBM;
                    oneLeg.maxWeight =  oneLeg.GrossWeight;
                    // firstLeg.CargoDescription = seaExportJob.CargoDescription;
                    oneLeg.CargoDescription = seaExportJob.SeaJobContainers[i].CargoDescription;
                    // 2014-07-14 Zhou Kai ends
                    oneLeg.LegType = Leg_Type.OneLeg;
                    oneLeg.PartnerLeg = 0;
                    //20150723 - geery replaced - to fixed bugs in planning that can set status to complete even if no assigned container and trailer
                    //oneLeg.isBillable = haulierJob.IsBillable ? true : false;
                    oneLeg.isBillable = true;


                    // 2014-07-09 Zhou Kai comments out the operation below:
                    //try
                    //{
                    //    haulierJob.AddHaulierJobTrip(firstLeg, "", "system");
                    //    haulierJobTrips.Add(firstLeg);
                    //}
                    //catch (FMException ex) { throw ex; }
                    //catch (Exception ex) { throw ex; }
                    // 2014-07-09 Zhou Kai ends

                    // 2014-07-09 Zhou Kai adds
                    oneLeg.IsLaden = false;
                    haulierJobTrips.Add(oneLeg);
                    // 2014-09-06 Zhou Kai ends
                }
            }
            else // 2-Legs Job
            {
                for (int i = 0; i < seaExportJob.SeaJobContainers.Count; i++)
                {
                    //20150825 - gery added checking if container has gross weight
                    if (seaExportJob.SeaJobContainers[i].GrossWeight <= 0) { throw new FMException("Gross weight should be greater than 0. "); }

                    HaulierJobTrip firstLeg = new HaulierJobTrip();
                    firstLeg.LegGroup = nLegGroup; // per container per leg_group
                    firstLeg.StartStop.Code = seaExportJob.PickUpPlaceCode;
                    firstLeg.StartStop.Description = seaExportJob.PickUpPlaceName;
                    firstLeg.StartStop.Address1 = seaExportJob.PickUpPlaceAdd1;
                    firstLeg.StartStop.Address2 = seaExportJob.PickUpPlaceAdd2;
                    firstLeg.StartStop.Address3 = seaExportJob.PickUpPlaceAdd3;
                    firstLeg.StartStop.Address4 = seaExportJob.PickUpPlaceAdd4;
                    firstLeg.EndStop.Code = seaExportJob.StuffingCode;
                    firstLeg.EndStop.Description = seaExportJob.StuffingName;
                    firstLeg.EndStop.Address1 = seaExportJob.StuffingAdd1;
                    firstLeg.EndStop.Address2 = seaExportJob.StuffingAdd2;
                    firstLeg.EndStop.Address3 = seaExportJob.StuffingAdd3;
                    firstLeg.EndStop.Address4 = seaExportJob.StuffingAdd4;

                    firstLeg.JobID = haulierJob.JobID;
                    firstLeg.Sequence = (i * 2) + 1;
                    firstLeg.StartDate = DateTime.Today;
                    firstLeg.EndDate = DateTime.Today;
                    firstLeg.StartTime = "0000";
                    firstLeg.EndTime = "0000";
                    firstLeg.TripStatus = JobTripStatus.Booked;
                    firstLeg.IsNew = true;
                    OperationDetail operationDetail = new OperationDetail();
                    operationDetail = OperationDetail.GetOperationDetail(seaExportJob.ShipperCode);
                    CustomerDTO customerDTO = new CustomerDTO();
                    if (operationDetail == null ||
                        operationDetail.customerCode == "")
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode("CASH");
                    }
                    else
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode(operationDetail.customerCode);
                    }

                    firstLeg.CustomerCode = customerDTO.Code;
                    firstLeg.CustomerName = customerDTO.Name;
                    firstLeg.OwnTransport = true;
                    //h1.SubContractorCode = "";   //20140312 - gerry replaced
                    //201040502 - gerry change subContractorCode from CustomerDTO to OperatorDTO
                    firstLeg.subCon = new OperatorDTO();//new CustomerDTO();
                    firstLeg.SubContractorReference = "";
                    firstLeg.ChargeCode = "";
                    firstLeg.Remarks = "";
                    firstLeg.IsLaden = false;
                    firstLeg.ContainerCode = seaExportJob.SeaJobContainers[i].ContainerCode;
                    ContainerTypesDTO containerTypesDTO = ContainerTypesDTO.GetContainerDTO(firstLeg.ContainerCode);
                    firstLeg.ContainerNo = seaExportJob.SeaJobContainers[i].ContainerNumber;
                    firstLeg.SealNo = seaExportJob.SeaJobContainers[i].SealNumber;
                    firstLeg.GrossWeight = seaExportJob.SeaJobContainers[i].GrossWeight; //normally empty container from yard
                    firstLeg.GrossCBM = seaExportJob.SeaJobContainers[i].GrossCBM == 0 ? containerTypesDTO.TareCBM : seaExportJob.SeaJobContainers[i].GrossCBM;
                    firstLeg.maxCBM = firstLeg.GrossCBM;
                    firstLeg.maxWeight = firstLeg.GrossWeight;
                    firstLeg.CargoDescription = seaExportJob.SeaJobContainers[i].CargoDescription;
                    firstLeg.LegType = Leg_Type.FirstOfTwoLeg;
                    firstLeg.PartnerLeg = (i * 2) + 2;
                    //20131127 -gerry added
                    firstLeg.isMulti_leg = true;
                    //20131127 end


                    // 2014-07-09 Zhou Kai comments out the operation below
                    //try
                    //{
                    //    haulierJob.AddHaulierJobTrip(firstLeg, "", "ipl");
                    //    haulierJobTrips.Add(firstLeg);
                    //}
                    //catch (FMException ex) { throw ex; }
                    //catch (Exception ex) { throw ex; }
                    // 2014-07-09 Zhou Kai ends

                    // 2014-07-09 Zhou Kai adds
                    //20150723 - geery replaced - to fixed bugs in planning that can set status to complete even if no assigned container and trailer
                    //firstLeg.isBillable = haulierJob.IsBillable ? true : false;                    
                    firstLeg.isBillable = true;
                    haulierJobTrips.Add(firstLeg);
                    // 2014-09-06 Zhou Kai ends

                    HaulierJobTrip secondLeg = new HaulierJobTrip();
                    secondLeg.LegGroup = nLegGroup++; // per container per leg_group
                    secondLeg.StartStop.Code = seaExportJob.StuffingCode;
                    secondLeg.StartStop.Description = seaExportJob.StuffingName;
                    secondLeg.StartStop.Address1 = seaExportJob.StuffingAdd1;
                    secondLeg.StartStop.Address2 = seaExportJob.StuffingAdd2;
                    secondLeg.StartStop.Address3 = seaExportJob.StuffingAdd3;
                    secondLeg.StartStop.Address4 = seaExportJob.StuffingAdd4;
                    secondLeg.EndStop.Code = seaExportJob.DeliveryCode;
                    secondLeg.EndStop.Description = seaExportJob.DeliveryName;
                    secondLeg.EndStop.Address1 = seaExportJob.DeliveryAdd1;
                    secondLeg.EndStop.Address2 = seaExportJob.DeliveryAdd2;
                    secondLeg.EndStop.Address3 = seaExportJob.DeliveryAdd3;
                    secondLeg.EndStop.Address4 = seaExportJob.DeliveryAdd4;

                    secondLeg.JobID = haulierJob.JobID;
                    secondLeg.Sequence = (i * 2) + 2;
                    secondLeg.StartDate = DateTime.Today;
                    secondLeg.EndDate = DateTime.Today;
                    secondLeg.StartTime = "0000";
                    secondLeg.EndTime = "0000";
                    secondLeg.TripStatus = JobTripStatus.Booked;
                    secondLeg.IsNew = true;
                    operationDetail = new OperationDetail();
                    operationDetail = OperationDetail.GetOperationDetail(seaExportJob.ShipperCode);
                    customerDTO = new CustomerDTO();
                    if (operationDetail == null || operationDetail.customerCode == "")
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode("CASH");
                    }
                    else
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode(operationDetail.customerCode);
                    }

                    secondLeg.CustomerCode = customerDTO.Code;
                    secondLeg.CustomerName = customerDTO.Name;
                    secondLeg.OwnTransport = true;
                    //h2.SubContractorCode = "";     //20140312 - gerry replaced
                    //201040502 - gerry change subContractorCode from CustomerDTO to OperatorDTO
                    secondLeg.subCon = new OperatorDTO();//new CustomerDTO();

                    secondLeg.SubContractorReference = "";
                    secondLeg.ChargeCode = "";
                    secondLeg.Remarks = "";

                    secondLeg.IsLaden = true;
                    secondLeg.ContainerNo = seaExportJob.SeaJobContainers[i].ContainerNumber;
                    secondLeg.SealNo = seaExportJob.SeaJobContainers[i].SealNumber;
                    secondLeg.GrossWeight = seaExportJob.SeaJobContainers[i].GrossWeight;
                    secondLeg.GrossCBM = seaExportJob.SeaJobContainers[i].GrossCBM == 0 ? containerTypesDTO.TareCBM : seaExportJob.SeaJobContainers[i].GrossCBM;
                    secondLeg.ContainerCode = seaExportJob.SeaJobContainers[i].ContainerCode;
                    // 2014-07-14 Zhou Kai modifies
                    // secondLeg.maxCBM = containerTypesDTO.TareCBM;
                    secondLeg.maxCBM = secondLeg.GrossCBM;
                    // secondLeg.maxWeight = containerTypesDTO.TareWeight;
                    secondLeg.maxWeight = secondLeg.GrossWeight;
                    // 2014-07-14 Zhou Kai ends
                    // secondLeg.CargoDescription = seaExportJob.CargoDescription;
                    secondLeg.CargoDescription = seaExportJob.SeaJobContainers[i].CargoDescription;
                    secondLeg.LegType = Leg_Type.SecondOfTwoLeg;
                    secondLeg.PartnerLeg = (i * 2) + 1;
                    //20131127 -gerry added
                    secondLeg.isMulti_leg = true;
                    //20131127 end

                    // 2014-07-09 Zhou Kai comments out the operation below
                    //try
                    //{
                    //    haulierJob.AddHaulierJobTrip(secondLeg, "", "ipl");
                    //    haulierJobTrips.Add(secondLeg);
                    //}
                    //catch (FMException ex) { throw ex; }
                    //catch (Exception ex) { throw ex; }
                    // 2014-07-09 Zhou Kai ends

                    // 2014-07-09 Zhou Kai adds
                    //20150723 - geery replaced - to fixed bugs in planning that can set status to complete even if no assigned container and trailer
                    //secondLeg.isBillable = haulierJob.IsBillable ? true : false;                    
                    secondLeg.isBillable = true;

                    haulierJobTrips.Add(secondLeg);
                    // 2014-07-09 Zhou Kai ends
                }
            }

            return haulierJobTrips;
        }

        public static SortableList<HaulierJobTrip> ConvertSeaImportContainerDataToHaulierTrip(SeaImportJob seaImportJob,
            HaulierJob haulierJob,Leg_Type legType)
        {
            SortableList<HaulierJobTrip> haulierJobTrips = new SortableList<HaulierJobTrip>();
            if (legType == Leg_Type.OneLeg)
            {
                int nLegGroup = 0;
                for (int i = 0; i < seaImportJob.SeaJobContainers.Count; i++)
                {
                    //20150825 - gery added checking if container has gross weight
                    if (seaImportJob.SeaJobContainers[i].GrossWeight <= 0) { throw new FMException("Gross weight should be greater than 0. "); }

                    HaulierJobTrip oneLeg = new HaulierJobTrip();
                    //OperatorDTO tmp = OperatorDTO.GetOperatorDTO(TransportSettings.GetTransportSetting().DefaultPortCode);
                    // 2014-07-17 Zhou Kai modifies below:
                    oneLeg.StartStop.Code = seaImportJob.PickUpPlaceCode;
                    oneLeg.StartStop.Description = seaImportJob.PickUpPlaceName;
                    oneLeg.StartStop.Address1 = seaImportJob.PickUpPlaceAdd1;
                    oneLeg.StartStop.Address2 = seaImportJob.PickUpPlaceAdd2;
                    oneLeg.StartStop.Address3 = seaImportJob.PickUpPlaceAdd3;
                    oneLeg.StartStop.Address4 = seaImportJob.PickUpPlaceAdd4;
                    oneLeg.EndStop.Code = seaImportJob.StuffingCode;
                    oneLeg.EndStop.Description = seaImportJob.StuffingName;
                    oneLeg.EndStop.Address1 = seaImportJob.StuffingAdd1;
                    oneLeg.EndStop.Address2 = seaImportJob.StuffingAdd2;
                    oneLeg.EndStop.Address3 = seaImportJob.StuffingAdd3;
                    oneLeg.EndStop.Address4 = seaImportJob.StuffingAdd4;

                    //oneLeg.StartStop.Code = tmp.Code;
                    //oneLeg.StartStop.Description = tmp.Name;
                    //oneLeg.StartStop.Address1 = tmp.Add1;
                    //oneLeg.StartStop.Address2 = tmp.Add2;
                    //oneLeg.StartStop.Address3 = tmp.Add3;
                    //oneLeg.StartStop.Address4 = tmp.Add4;
                    //oneLeg.StartStop.City = tmp.City;
                    //oneLeg.EndStop.Code = seaImportJob.EmptyContainerToCode;
                    //oneLeg.EndStop.Description = seaImportJob.EmptyContainerToName;
                    //oneLeg.EndStop.Address1 = seaImportJob.EmptyContainerToAdd1;
                    //oneLeg.EndStop.Address2 = seaImportJob.EmptyContainerToAdd2;
                    //oneLeg.EndStop.Address3 = seaImportJob.EmptyContainerToAdd3;
                    //oneLeg.EndStop.Address4 = seaImportJob.EmptyContainerToAdd4;
                    //oneLeg.EndStop.City = seaImportJob.EmptyContainerToCityCode;
                    // 2014-07-17 Zhou Kai ends

                    oneLeg.JobID = haulierJob.JobID;
                    oneLeg.LegGroup = ++nLegGroup;
                    oneLeg.Sequence = i + 1;
                    // 2014-07-02 Zhou Kai modifies the two lines below:
                    //haulierOneLegTrip.StartDate = DateTime.Today;
                    //haulierOneLegTrip.EndDate = DateTime.Today;
                    oneLeg.StartDate = haulierJob.eta;
                    oneLeg.EndDate = haulierJob.eta;
                    // 2014-07-02 Zhou Kai ends
                    oneLeg.StartTime = "0000";
                    oneLeg.EndTime = "0000";
                    oneLeg.TripStatus = JobTripStatus.Booked;
                    oneLeg.IsNew = true;
                    OperationDetail operationDetail = new OperationDetail();
                    operationDetail = OperationDetail.GetOperationDetail(seaImportJob.ConsigneeCode);
                    CustomerDTO customerDTO = new CustomerDTO();
                    if (operationDetail == null ||
                        operationDetail.customerCode == "")
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode("CASH");
                    }
                    else
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode(operationDetail.customerCode);
                    }

                    oneLeg.CustomerCode = customerDTO.Code;
                    oneLeg.CustomerName = customerDTO.Name;
                    oneLeg.OwnTransport = true;
                    //h1.SubContractorCode = "";   //20140312 - gerry replaced
                    //201040502 - gerry change subContractorCode from CustomerDTO to OperatorDTO
                    oneLeg.subCon = new OperatorDTO();//new CustomerDTO();

                    oneLeg.SubContractorReference = "";
                    oneLeg.ChargeCode = "";
                    oneLeg.Remarks = "";

                    oneLeg.ContainerCode = seaImportJob.SeaJobContainers[i].ContainerCode;                 
                    ContainerTypesDTO containerTypesDTO =  ContainerTypesDTO.GetContainerDTO(oneLeg.ContainerCode);
                    oneLeg.ContainerNo = seaImportJob.SeaJobContainers[i].ContainerNumber;
                    oneLeg.SealNo = seaImportJob.SeaJobContainers[i].SealNumber;
                    oneLeg.GrossWeight = seaImportJob.SeaJobContainers[i].GrossWeight;
                    oneLeg.GrossCBM = seaImportJob.SeaJobContainers[i].GrossCBM == 0 ? containerTypesDTO.TareCBM : seaImportJob.SeaJobContainers[i].GrossCBM;
                     // 2014-07-14 Zhou Kai modifies
                    // haulierOneLegTrip.maxCBM = containerTypesDTO.TareCBM;
                    oneLeg.maxCBM = oneLeg.GrossCBM;
                    // haulierOneLegTrip.maxWeight = containerTypesDTO.TareWeight;
                    oneLeg.maxWeight = oneLeg.GrossWeight; // -containerTypesDTO.TareWeight;
                    // 2014-07-14 Zhou Kai ends
                    // 2014-07-02 Zhou Kai modifies the line below:
                    // haulierOneLegTrip.CargoDescription = seaImportJob.CargoDescription;
                    oneLeg.CargoDescription = seaImportJob.SeaJobContainers[i].CargoDescription;
                    // 2014-07-02 Zhou Kai ends
                    oneLeg.LegType = Leg_Type.OneLeg;
                    oneLeg.PartnerLeg = 0;

                    // 2014-07-09 Zhou Kai comments out the operation below:
                    //try
                    //{
                    //    haulierJob.AddHaulierJobTrip(haulierOneLegTrip, "", "ipl");
                    //    haulierJobTrips.Add(haulierOneLegTrip);
                    //}
                    //catch (FMException ex) { throw ex; }
                    //catch (Exception ex) { throw ex; }
                    // 2014-07-09 Zhou Kai ends

                    // 2014-07-09 Zhou Kai adds
                    //20150723 - geery replaced - to fixed bugs in planning that can set status to complete even if no assigned container and trailer
                    //oneLeg.isBillable = haulierJob.IsBillable ? true : false;
                    oneLeg.isBillable = true;

                    oneLeg.IsLaden = true;
                    haulierJobTrips.Add(oneLeg);
                    // 2014-09-06 Zhou Kai ends
                }
            }
            else // 2-Legs Job
            {
                int nLegGroup = 1;
                for (int i = 0; i < seaImportJob.SeaJobContainers.Count; i++)
                {
                    //20150825 - gery added checking if container has gross weight
                    if (seaImportJob.SeaJobContainers[i].GrossWeight <= 0) { throw new FMException("Gross weight should be greater than 0. "); }

                    HaulierJobTrip firstLeg = new HaulierJobTrip();
                    // 2014-07-17 Zhou Kai modifies
                    firstLeg.StartStop.Code = seaImportJob.PickUpPlaceCode;
                    firstLeg.StartStop.Description = seaImportJob.PickUpPlaceName;
                    firstLeg.StartStop.Address1 = seaImportJob.PickUpPlaceAdd1;
                    firstLeg.StartStop.Address2 = seaImportJob.PickUpPlaceAdd2;
                    firstLeg.StartStop.Address3 = seaImportJob.PickUpPlaceAdd3;
                    firstLeg.StartStop.Address4 = seaImportJob.PickUpPlaceAdd4;
                    firstLeg.EndStop.Code = seaImportJob.StuffingCode;
                    firstLeg.EndStop.Description = seaImportJob.StuffingName;
                    firstLeg.EndStop.Address1 = seaImportJob.StuffingAdd1;
                    firstLeg.EndStop.Address2 = seaImportJob.StuffingAdd2;
                    firstLeg.EndStop.Address3 = seaImportJob.StuffingAdd3;
                    firstLeg.EndStop.Address4 = seaImportJob.StuffingAdd4;

                    //firstLeg.EndStop.Code = seaImportJob.PickUpPlaceCode;
                    //firstLeg.EndStop.Description = seaImportJob.PickUpPlaceName;
                    //firstLeg.EndStop.Address1 = seaImportJob.PickUpPlaceAdd1;
                    //firstLeg.EndStop.Address2 = seaImportJob.PickUpPlaceAdd2;
                    //firstLeg.EndStop.Address3 = seaImportJob.PickUpPlaceAdd3;
                    //firstLeg.EndStop.Address4 = seaImportJob.PickUpPlaceAdd4;
                    //firstLeg.EndStop.City = seaImportJob.PickUpPlaceCityCode;
                    ////OperatorDTO tmp = OperatorDTO.GetOperatorDTO(TransportSettings.GetTransportSetting().DefaultPortCode);
                    ////Stop port = new Stop(tmp.Code, tmp.Name, tmp.Add1, tmp.Add2, tmp.Add3, tmp.Add4, tmp.City);
                    //firstLeg.StartStop.Code = port.Code;
                    //firstLeg.StartStop.Description = port.Description;
                    //firstLeg.StartStop.Address1 = port.Address1;
                    //firstLeg.StartStop.Address2 = port.Address2;
                    //firstLeg.StartStop.Address3 = port.Address3;
                    //firstLeg.StartStop.Address4 = port.Address4;
                    //firstLeg.StartStop.City = port.City;
                    // 2014-07-17 Zhou Kai ends

                    firstLeg.JobID = haulierJob.JobID;
                    firstLeg.LegGroup = nLegGroup;
                    firstLeg.Sequence = i + 1;
                    firstLeg.StartDate = DateTime.Today;
                    firstLeg.EndDate = DateTime.Today;
                    firstLeg.StartTime = "0000";
                    firstLeg.EndTime = "0000";
                    firstLeg.TripStatus = JobTripStatus.Booked;
                    firstLeg.IsNew = true;
                    OperationDetail operationDetail = new OperationDetail();
                    operationDetail = OperationDetail.GetOperationDetail(seaImportJob.ConsigneeCode);
                    CustomerDTO customerDTO = new CustomerDTO();
                    if (operationDetail == null ||
                        operationDetail.customerCode == "")
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode("CASH");
                    }
                    else
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode(operationDetail.customerCode);
                    }
                    firstLeg.CustomerCode = customerDTO.Code;
                    firstLeg.CustomerName = customerDTO.Name;
                    firstLeg.OwnTransport = true;
                    //h1.SubContractorCode = "";     //20140312 - gerry replaced
                    //201040502 - gerry change subContractorCode from CustomerDTO to OperatorDTO
                    firstLeg.subCon = new OperatorDTO();//new CustomerDTO();
                    firstLeg.SubContractorReference = "";
                    firstLeg.ChargeCode = "";
                    firstLeg.Remarks = "";
                    firstLeg.ContainerCode = seaImportJob.SeaJobContainers[i].ContainerCode;
                    ContainerTypesDTO containerTypesDTO = ContainerTypesDTO.GetContainerDTO(firstLeg.ContainerCode);
                    firstLeg.ContainerNo = seaImportJob.SeaJobContainers[i].ContainerNumber;
                    firstLeg.SealNo = seaImportJob.SeaJobContainers[i].SealNumber;
                    firstLeg.GrossWeight = seaImportJob.SeaJobContainers[i].GrossWeight;
                    firstLeg.GrossCBM = seaImportJob.SeaJobContainers[i].GrossCBM == 0 ? containerTypesDTO.TareCBM : seaImportJob.SeaJobContainers[i].GrossCBM;
                    // 2014-07-02 Zhou Kai modifies the line below:
                    firstLeg.CargoDescription = seaImportJob.SeaJobContainers[i].CargoDescription;
                    firstLeg.LegType = Leg_Type.FirstOfTwoLeg;
                    firstLeg.PartnerLeg = i + 2;
                    //20131127 -gerry added
                    firstLeg.isMulti_leg = true;
                    //20131127 end

                    // 2014-07-14 Zhou Kai modifies
                    firstLeg.maxCBM = firstLeg.GrossCBM;
                    // firstLeg.maxCBM = containerTypesDTO.TareCBM;
                    firstLeg.maxWeight = seaImportJob.SeaJobContainers[i].GrossWeight;
                    // 2014-07-09 Zhou Kai comments out the operation below
                    //try
                    //{
                    //    haulierJob.AddHaulierJobTrip(firstLeg, "", "ipl");
                    //    haulierJobTrips.Add(firstLeg);
                    //}
                    //catch (FMException ex) { throw ex; }
                    //catch (Exception ex) { throw ex; }
                    // 2014-07-09 Zhou Kai ends

                    //20150723 - geery replaced - to fixed bugs in planning that can set status to complete even if no assigned container and trailer
                    //firstLeg.isBillable = haulierJob.IsBillable ? true : false;
                    firstLeg.isBillable = true;

                    // 2014-07-09 Zhou Kai adds
                    firstLeg.IsLaden = true;
                    haulierJobTrips.Add(firstLeg);
                    // 2014-09-06 Zhou Kai ends

                    HaulierJobTrip secondLeg = new HaulierJobTrip();
                    // 2014-07-16 Zhou Kai modifies the codes below:
                    secondLeg.StartStop.Code = seaImportJob.StuffingCode;
                    secondLeg.StartStop.Description = seaImportJob.StuffingName;
                    secondLeg.StartStop.Address1 = seaImportJob.StuffingAdd1;
                    secondLeg.StartStop.Address2 = seaImportJob.StuffingAdd2;
                    secondLeg.StartStop.Address3 = seaImportJob.StuffingAdd3;
                    secondLeg.StartStop.Address4 = seaImportJob.StuffingAdd4;
                    secondLeg.StartStop.City = seaImportJob.StuffingCityCode;
                    secondLeg.EndStop.Code = seaImportJob.DeliveryCode;
                    secondLeg.EndStop.Description = seaImportJob.DeliveryName;
                    secondLeg.EndStop.Address1 = seaImportJob.DeliveryAdd1;
                    secondLeg.EndStop.Address2 = seaImportJob.DeliveryAdd2;
                    secondLeg.EndStop.Address3 = seaImportJob.DeliveryAdd3;
                    secondLeg.EndStop.Address4 = seaImportJob.DeliveryAdd4;
                    secondLeg.EndStop.City = seaImportJob.DeliveryCityCode;

                    //secondLeg.StartStop.Code = seaImportJob.PickUpPlaceCode;
                    //secondLeg.StartStop.Description = seaImportJob.PickUpPlaceName;
                    //secondLeg.StartStop.Address1 = seaImportJob.PickUpPlaceAdd1;
                    //secondLeg.StartStop.Address2 = seaImportJob.PickUpPlaceAdd2;
                    //secondLeg.StartStop.Address3 = seaImportJob.PickUpPlaceAdd3;
                    //secondLeg.StartStop.Address4 = seaImportJob.PickUpPlaceAdd4;
                    //secondLeg.StartStop.City = seaImportJob.PickUpPlaceCityCode;
                    //secondLeg.EndStop.Code = seaImportJob.EmptyContainerToCode; ;
                    //secondLeg.EndStop.Description = seaImportJob.EmptyContainerToName;
                    //secondLeg.EndStop.Address1 = seaImportJob.EmptyContainerToAdd1;
                    //secondLeg.EndStop.Address2 = seaImportJob.EmptyContainerToAdd2;
                    //secondLeg.EndStop.Address3 = seaImportJob.EmptyContainerToAdd3;
                    //secondLeg.EndStop.Address4 = seaImportJob.EmptyContainerToAdd4;
                    //secondLeg.EndStop.City = seaImportJob.EmptyContainerToCityCode;
                    // 2014-07-16 Zhou Kai ends
                    secondLeg.JobID = haulierJob.JobID;
                    secondLeg.LegGroup = nLegGroup++;
                    secondLeg.Sequence = i + 2;
                    secondLeg.StartDate = DateTime.Today;
                    secondLeg.EndDate = DateTime.Today;
                    secondLeg.StartTime = "0000";
                    secondLeg.EndTime = "0000";
                    secondLeg.TripStatus = JobTripStatus.Booked;
                    secondLeg.IsNew = true;
                    operationDetail = new OperationDetail();
                    operationDetail = OperationDetail.GetOperationDetail(seaImportJob.ConsigneeCode);
                    customerDTO = new CustomerDTO();
                    if (operationDetail == null ||
                        operationDetail.customerCode == "")
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode("CASH");
                    }
                    else
                    {
                        customerDTO = CustomerDTO.GetCustomerByCustCode(operationDetail.customerCode);
                    }
                    secondLeg.CustomerCode = customerDTO.Code;
                    secondLeg.CustomerName = customerDTO.Name;
                    secondLeg.OwnTransport = true;
                    //h2.SubContractorCode = "";    //20140312 - gerry replaced
                    //201040502 - gerry change subContractorCode from CustomerDTO to OperatorDTO
                    secondLeg.subCon = new OperatorDTO();//new CustomerDTO();
                    secondLeg.SubContractorReference = "";
                    secondLeg.ChargeCode = "";
                    secondLeg.Remarks = "";
                    secondLeg.ContainerCode = seaImportJob.SeaJobContainers[i].ContainerCode;
                    secondLeg.ContainerNo = seaImportJob.SeaJobContainers[i].ContainerNumber;
                    secondLeg.SealNo = seaImportJob.SeaJobContainers[i].SealNumber;
                    secondLeg.GrossWeight = firstLeg.GrossWeight; //normally going to container yard
                    secondLeg.GrossCBM = seaImportJob.SeaJobContainers[i].GrossCBM == 0 ? containerTypesDTO.TareCBM : seaImportJob.SeaJobContainers[i].GrossCBM;
                    // 2014-07-02 Zhou Kai modifies the line below:
                    secondLeg.CargoDescription = seaImportJob.SeaJobContainers[i].CargoDescription;
                    secondLeg.LegType = Leg_Type.SecondOfTwoLeg;
                    secondLeg.PartnerLeg = firstLeg.Sequence;//i + 1;
                    //20131127 -gerry added
                    secondLeg.isMulti_leg = true;
                    //20131127 end

                    secondLeg.maxCBM = secondLeg.GrossCBM;
                    secondLeg.maxWeight = secondLeg.GrossWeight;
                   
                    // 2014-07-09 Zhou Kai comments out the operation below
                    //try
                    //{
                    //    haulierJob.AddHaulierJobTrip(secondLeg, "", "ipl");
                    //    haulierJobTrips.Add(secondLeg);
                    //}
                    //catch (FMException ex) { throw ex; }
                    //catch (Exception ex) { throw ex; }
                    // // 2014-07-09 Zhou Kai ends

                    //20150723 - geery replaced - to fixed bugs in planning that can set status to complete even if no assigned container and trailer
                    //secondLeg.isBillable = haulierJob.IsBillable ? true : false;
                    secondLeg.isBillable = true;
                    // 2014-07-09 Zhou Kai adds
                    secondLeg.IsLaden = false;
                    haulierJobTrips.Add(secondLeg);
                    // 2014-09-06 Zhou Kai ends
                }
            }

            return haulierJobTrips;
        }
        //20160114 - gerry overload method to convert containers in job trip
        public static SortableList<HaulierJobTrip> ConvertSeaImportContainerDataToHaulierTrip(SeaImportJob seaImportJob,
            HaulierJob haulierJob, Leg_Type legType, out string containerCheckMessage)
        {
            SortableList<HaulierJobTrip> haulierJobTrips = new SortableList<HaulierJobTrip>();
            containerCheckMessage = string.Empty;
            int nLegGroup = 1;
            foreach (SeaJobContainer container in seaImportJob.SeaJobContainers)
            {
                //20150825 - gery added checking if container has gross weight
                if (container.GrossWeight <= 0) { throw new FMException("Gross weight should be greater than 0. "); }
                OperationDetail operationDetail = new OperationDetail();
                operationDetail = OperationDetail.GetOperationDetail(seaImportJob.ConsigneeCode);
                CustomerDTO customerDTO = new CustomerDTO();
                if (operationDetail == null || operationDetail.customerCode == "")
                    customerDTO = CustomerDTO.GetCustomerByCustCode("CASH");
                else
                    customerDTO = CustomerDTO.GetCustomerByCustCode(operationDetail.customerCode);

                ContainerTypesDTO containerTypesDTO = ContainerTypesDTO.GetContainerDTO(container.ContainerCode);

                #region Common Trip properties
                HaulierJobTrip jobTrip = new HaulierJobTrip();
                jobTrip.JobID = haulierJob.JobID;
                jobTrip.Sequence += 1;
                jobTrip.StartStop.Code = seaImportJob.PickUpPlaceCode;
                jobTrip.StartStop.Description = seaImportJob.PickUpPlaceName;
                jobTrip.StartStop.Address1 = seaImportJob.PickUpPlaceAdd1;
                jobTrip.StartStop.Address2 = seaImportJob.PickUpPlaceAdd2;
                jobTrip.StartStop.Address3 = seaImportJob.PickUpPlaceAdd3;
                jobTrip.StartStop.Address4 = seaImportJob.PickUpPlaceAdd4;
                jobTrip.EndStop.Code = seaImportJob.StuffingCode;
                jobTrip.EndStop.Description = seaImportJob.StuffingName;
                jobTrip.EndStop.Address1 = seaImportJob.StuffingAdd1;
                jobTrip.EndStop.Address2 = seaImportJob.StuffingAdd2;
                jobTrip.EndStop.Address3 = seaImportJob.StuffingAdd3;
                jobTrip.EndStop.Address4 = seaImportJob.StuffingAdd4;
                jobTrip.CustomerCode = customerDTO.Code;
                jobTrip.CustomerName = customerDTO.Name;
                jobTrip.StartDate = haulierJob.eta;
                jobTrip.EndDate = haulierJob.eta;
                jobTrip.StartTime = "0000";
                jobTrip.EndTime = "0000";
                jobTrip.TripStatus = JobTripStatus.Booked;
                jobTrip.IsNew = true;
                jobTrip.OwnTransport = true;
                jobTrip.subCon = new OperatorDTO();//new CustomerDTO();
                jobTrip.SubContractorReference = "";
                jobTrip.ChargeCode = "";
                jobTrip.Remarks = "";
                jobTrip.ContainerCode = container.ContainerCode;
                jobTrip.isBillable = true;
                jobTrip.IsLaden = true;
                jobTrip.ContainerNo = container.ContainerNumber;
                jobTrip.SealNo = container.SealNumber;
                jobTrip.GrossWeight = container.GrossWeight;
                jobTrip.GrossCBM = container.GrossCBM;
                jobTrip.maxCBM = jobTrip.GrossCBM;
                jobTrip.maxWeight = jobTrip.GrossWeight;
                jobTrip.CargoDescription = container.CargoDescription;
                jobTrip.vesselNo = haulierJob.VesselNo;
                jobTrip.vesselName = haulierJob.VesselName;
                jobTrip.voyageNo = haulierJob.voyageNo;
                jobTrip.SourceRefNo = haulierJob.SourceReferenceNo;
                #endregion
                //other differ based on leg type
                switch (legType)
                {
                    case Leg_Type.OneLeg:
                        #region Single Leg Trip
                        jobTrip.isMulti_leg = false;
                        jobTrip.LegType = Leg_Type.OneLeg;
                        jobTrip.LegGroup++;
                        jobTrip.LegGroupMember = 1;
                        jobTrip.PartnerLeg = 0;
                        haulierJobTrips.Add(jobTrip);
                        #endregion
                        break;
                    default: //normally 2 legs trip as default let the user add more trip after convert
                        #region First Leg
                        jobTrip.isMulti_leg = true;
                        jobTrip.LegType = Leg_Type.FirstOfTwoLeg;
                        jobTrip.LegGroup++;
                        jobTrip.LegGroupMember = 1;
                        jobTrip.PartnerLeg = jobTrip.Sequence + 1;
                        haulierJobTrips.Add(jobTrip);
                        #endregion

                        #region Second Leg
                        //HaulierJobTrip secondLegTrip = jobTrip.MemberwiseClone();
                        jobTrip.isMulti_leg = true;
                        jobTrip.LegType = Leg_Type.SecondOfTwoLeg;
                        jobTrip.LegGroup++;
                        jobTrip.LegGroupMember = 2;
                        jobTrip.PartnerLeg = jobTrip.Sequence - 1;
                        jobTrip.GrossWeight = 0;
                        jobTrip.GrossCBM = 0;
                        jobTrip.maxCBM = 0;
                        jobTrip.maxWeight = 0;
                        haulierJobTrips.Add(jobTrip);
                        #endregion
                        break;
                }
            }
            return haulierJobTrips;
        }
        // Chong Chin 22 April 2010 - Start 
        public static List<string> GetAllCustomerCodes()
        {
            return CustomerDTO.GetAllCustomerCodes();
        }

        public static List<string> GetAllOperatorCode()
        {
            return OperatorDTO.GetAllOperatorCodes();
        }

        public static List<string> GetAllCustomerNames()
        {
            return CustomerDTO.GetAllCustomerNames();
        }

        public static List<string> GetAllOperatorNames()
        {
            return OperatorDTO.GetAllOperatorNames();
        }

        public static OperatorDTO GetOperatorDTO(string operatorDTOCode)
        {
            return OperatorDTO.GetOperatorDTO(operatorDTOCode);
        }

        public static OperatorDTO GetOperatorDTOByName(string operatorDTOName)
        {
            return OperatorDTO.GetOperatorDTOByName(operatorDTOName);
        }

        // Chong Chin 22 April End 

        //16 Aug, 2011 Gerry Added
        public static List<string> GetContainerSizes()
        {
            return ContainerTypesDTO.GetContainerSizes();
        }

        // 2014-07-30 Zhou Kai adds the parameter noOfLegs
        public static List<string> GetContainerCodesBySizeUsedInQuotationAndJobTrips(int quotationID, bool stopDependent, int noOfLegs)
        {
            return ContainerTypesDTO.GetContainerCodesBySizeUsedInQuotationAndJobTrips(quotationID, stopDependent, noOfLegs);
        }

        public static List<string> GetAllContainerCodes()
        {
            return ContainerTypesDTO.GetAllContainerCodes();
        }

        public static string GetContainerSizeByContainerCode(string containerCode)
        {
            return ContainerTypesDTO.GetContainerDTO(containerCode).Size;
        }
        //16 Aug, 2011 End

        // 2014-07-30 Zhou Kai adds parameter noOfLegs to this function
        public static List<ContainerTypesDTO> GetContainerDTOBySizeUsedInQuotation(int quotationID, bool stopDependent, int noOfLegs)
        {
            return ContainerTypesDTO.GetContainerDTOBySizeUsedInQuotationAndJobTrips(quotationID, stopDependent, noOfLegs);
        }

        /// <summary>
        /// 2014-08-14 Zhou Kai adds, this facade in function get the container 
        /// code -> size mapping via 
        /// ContainerTypesDTO.GetContainerCodeSizeMapping() which then goes
        /// through SeaFreightFacadeOut, then goes to SeaFreightDAL, and
        /// finally returns data to this function.
        /// </summary>
        /// <returns>The container code -> size mapping dictionary</returns>
        public static Dictionary<string, string> GetContainerCodeSizeMapping()
        {
            return SeaFreightFacadeOut.GetContainerCodeSizeMapping();
        }

        /// <summary>
        /// 2014-08-29 Zhou Kai adds
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ContainerTypesDTO GetContainerDTO(string code)
        {
            return SeaFreightFacadeOut.GetContainerDTO(code);
        }
        //20150817 - gerry added for container number checking based on ISO standard
        public static bool IsValidContainerNoBasedOnISO(string contNo)
        {
            return ContainerTypesDTO.CheckValidContainerNo(contNo);
        }

    }
}
