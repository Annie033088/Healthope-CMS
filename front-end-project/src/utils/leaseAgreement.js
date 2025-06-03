export const leaseAgreementStatus = {
    // 未啟用
    Inactive: 1,
    // 啟用中
    Active: 2,
    // 已完成
    Completed: 3,
    // 取消
    Cancel: 4,
}

export const leaseAgreementStatusAndText = [
    { value: '1', text: '未啟用' },
    { value: '2', text: '啟用中' },
    { value: '3', text: '已完成' },
    { value: '4', text: '取消' },
]

export function leaseAgreementStatusTranslateTable(oldStatus, newStatus) {
    oldStatus = Number(oldStatus)
    newStatus = Number(newStatus)

    if (oldStatus === leaseAgreementStatus.Inactive) {
        if (newStatus === leaseAgreementStatus.Active) return true;
        else false;
    }

    if (oldStatus === leaseAgreementStatus.Active) {
        if (newStatus === leaseAgreementStatus.Completed) return true;
        else if (newStatus === leaseAgreementStatus.Cancel) return true;
        else false;
    }

    return false;
}