export const refundStatus = {
    // 待處理
    Pending: 1,
    // 已處理
    Processed: 2,
    // 失敗
    Fail: 3,
}

export const refundStatusAndText = [
    { value: "1", text: '待處理' },
    { value: "2", text: '已處理' },
    { value: "3", text: '失敗' },
]

export const refundType = {
    // 解約
    Terminate: 1,
    // 違約
    Breach: 2,
    // 7 日內退款
    RefundIn7Days: 2,
}

export const refundTypeAndText = [
    { value: "1", text: '解約' },
    { value: "2", text: '違約' },
    { value: "3", text: '7 日內退款' },
]
