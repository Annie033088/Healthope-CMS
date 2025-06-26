export const electronicInvoiceStatus = {
    // 處理中
    Processing: 1,
    // (開立)成功
    Success: 2,
    // (開立)失敗
    Fail: 3,
    // 待作廢
    PendingVoid: 4,
    // 已作廢
    Voided: 5,
    // 待折讓
    PendingDiscount: 6,
    // 已折讓
    Discounted: 7,
}

export const electronicInvoiceStatusAndText = [
    { value: "1", text: '處理中' },
    { value: "2", text: '成功' },
    { value: "3", text: '失敗' },
    { value: "4", text: '待作廢' },
    { value: "5", text: '已作廢' },
    { value: "6", text: '待折讓' },
    { value: "7", text: '已折讓' },
]