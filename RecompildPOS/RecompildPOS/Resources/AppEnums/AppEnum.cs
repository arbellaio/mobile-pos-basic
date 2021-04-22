using System;
using System.Collections.Generic;
using System.Text;

namespace RecompildPOS.Resources.AppEnums
{
    public class AppEnum
    {
        public enum OrderStatusEnum
        {
            Active = 1,
            Complete = 2,
            Hold = 3,
            Pending = 4,
            NotScheduled = 5,
            Scheduled = 6,
            ReAllocationRequired = 7,
            AwaitingAuthorisation = 8,
            Cancelled = 9,
            BeingPicked = 10
        }

        public enum OrderProcessStatusEnum
        {
            Active = 1,
            Complete = 2,
            Dispatched = 3,
            Loaded = 4,
            Delivered = 5,
            Invoiced = 6,
            PostedToAccounts = 7
        }

        public enum InventoryTransactionTypeEnum
        {
            PurchaseOrder = 1,
            SaleOrder = 2,
            TransferIn = 3,
            TransferOut = 4,
            Allocated = 5,
            AdjustmentIn = 6,
            AdjustmentOut = 7,
            WorkOrder = 8,
            Proforma = 9,
            Quotation = 10,
            Loan = 11,
            Returns = 12,
            Samples = 13,
            Wastage = 14,
            DirectSales = 15,
            Exchange = 16,
            WastedReturn = 17
        }

        public enum BarCodeEnum
        {
            
        }
    }
}
