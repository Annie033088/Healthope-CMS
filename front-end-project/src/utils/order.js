export const orderState = {
    // 待付款
    Pending: 1,
    // 已付款
    Paid: 2,
    // 取消
    Cancel: 3,
    // 解約
    Terminate: 4,
    // 違約
    Breach: 5,
    // 付款處理中
    Paying: 6,
}

export const orderStateAndText = [
    { value: "1", text: '待付款' },
    { value: "2", text: '已付款' },
    { value: "3", text: '取消' },
    { value: "4", text: '解約' },
    { value: "5", text: '違約' },
    { value: "6", text: '付款處理中' },
]

export const paymentMethod = {
    // 現金
    Cash: 1,
    // 信用卡
    Card: 2,
}

export const paymentMethodAndText = [
    { value: "1", text: '現金' },
    { value: "2", text: '信用卡' },
]