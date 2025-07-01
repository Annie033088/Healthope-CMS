export const transactionStatus = {
    // 處理中
    Processing: 1,
    // 成功
    Success: 2,
    // 失敗
    Fail: 3,
}

export const transactionStatusAndText = [
    { value: "1", text: '處理中' },
    { value: "2", text: '成功' },
    { value: "3", text: '失敗' },
]

export const transactionMethod = {
    // 現金
    Cash: 1,
    // 信用卡
    Card: 2,
}

export const transactionMethodAndText = [
    { value: "1", text: '現金' },
    { value: "2", text: '信用卡' },
]
