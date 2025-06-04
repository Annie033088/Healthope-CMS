export default function (mock) {
    const leaseAgreements = [
        {
            LeaseAgreementId: 101,
            StartTime: "2025-06-01",
            EndTime: "2025-12-31",
            Remind: true,
            Remark: "Monthly renewal",
            Status: 2,
            ReminderLeadTime: 7,
            CreateTime: "2025-06-01T09:30:00.123",
            UpdateTime: "2025-06-03T10:45:00.456"
        },
        {
            LeaseAgreementId: 102,
            StartTime: "2025-07-01",
            EndTime: "2025-12-01",
            Remind: false,
            Remark: "One-time lease",
            Status: 3,
            ReminderLeadTime: 0,
            CreateTime: "2025-06-02T08:20:00.000",
            UpdateTime: "2025-06-03T11:00:00.000"
        },
        {
            LeaseAgreementId: 103,
            StartTime: "2025-05-15",
            EndTime: "2026-05-14",
            Remind: true,
            Remark: "Annual contract",
            Status: 2,
            ReminderLeadTime: 14,
            CreateTime: "2025-05-15T13:45:00.321",
            UpdateTime: "2025-06-01T14:00:00.100"
        },
        {
            LeaseAgreementId: 104,
            StartTime: "2025-04-01",
            EndTime: "2025-10-01",
            Remind: false,
            Remark: "Half-year deal",
            Status: 4,
            ReminderLeadTime: 0,
            CreateTime: "2025-04-01T10:10:10.010",
            UpdateTime: "2025-05-10T10:15:10.999"
        },
        {
            LeaseAgreementId: 105,
            StartTime: "2025-06-10",
            EndTime: "2025-08-10",
            Remind: true,
            Remark: "Short term",
            Status: 1,
            ReminderLeadTime: 3,
            CreateTime: "2025-06-03T09:00:00.000",
            UpdateTime: "2025-06-03T09:10:00.000"
        },
        {
            LeaseAgreementId: 106,
            StartTime: "2025-03-10",
            EndTime: "2025-05-10",
            Remind: false,
            Remark: "Short term",
            Status: 1,
            ReminderLeadTime: 3,
            CreateTime: "2025-06-03T09:00:00.000",
            UpdateTime: "2025-06-03T09:10:00.000"
        }
    ];

    mock.onPost("/api/LeaseAgreement/AddLeaseAgreement").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/LeaseAgreement/GetLeaseAgreement").reply(config => {
        let {
            Status,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = leaseAgreements.filter(item => {
            const matchStatus = Status === null || item.Status === Number(Status);
            return matchStatus;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const paged = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            LeaseAgreementList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/LeaseAgreement/DeleteLeaseAgreement").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/LeaseAgreement/EditLeaseAgreementStatus").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/LeaseAgreement/EditLeaseAgreementRemind").reply(() => {
        return [200, { ErrorCode: 1 }]
    })
}