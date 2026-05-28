using System;

namespace CleanRefactor
{
    public sealed class PurchaseItemResponse
    {
        public PurchaseItemStatus Status { get; }
        public bool Success => Status == PurchaseItemStatus.Success;
        public string Message { get; }

        public PurchaseItemResponse(PurchaseItemStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}

