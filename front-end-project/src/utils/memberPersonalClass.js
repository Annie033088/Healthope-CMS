export const memberPersonalClassStatus = {
    // 預約中
    BookingInProgress: 1,
    // 預約成功
    BookedSuccessfully: 2,
    // 未出席
    DidNotAttend: 3,
    // 已出席
    Attended: 4,
    // 取消
    Cancelled: 5,
}

export const memberPersonalClassStatusAndText = [
    { value: "1", text: '預約中' },
    { value: "2", text: '預約成功' },
    { value: "3", text: '未出席' },
    { value: "4", text: '已出席' },
    { value: "5", text: '取消' },
]

export const memberPersonalClassCategory = {
    // 體驗課程
    TrialCourses: false,
    // 付費課程
    PaidCourses: true,
}

export const memberPersonalClassCategoryAndText = [
    { value: false, text: '體驗課程' },
    { value: true, text: '付費課程' },
]