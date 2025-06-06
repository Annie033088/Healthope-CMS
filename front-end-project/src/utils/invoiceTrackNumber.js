export const invoiceTrackNumberStatus = {
    // 未啟用
    Inactive: 1,
    // 啟用中
    Active: 2,
    // 已停用
    Disabled: 3,
    // 結束
    Closed: 4,
}

export const invoiceTrackNumberStatusAndText = [
    { value: "1", text: '未啟用' },
    { value: "2", text: '啟用中' },
    { value: "3", text: '已停用' },
    { value: "4", text: '結束' },
]


export function invoiceTrackNumberStatusTranslateTable(oldStatus, newStatus) {
    oldStatus = Number(oldStatus)
    newStatus = Number(newStatus)

    if (oldStatus === invoiceTrackNumberStatus.Inactive) {
        if (newStatus === invoiceTrackNumberStatus.Active) return true;

        return false;
    }

    if (oldStatus === invoiceTrackNumberStatus.Active) {
        if (newStatus === invoiceTrackNumberStatus.Disabled) return true;
        
        return false;
    }

    return false;
}