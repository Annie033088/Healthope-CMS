export default function (mock) {
    const refunds = [
        {
            "RefundId": 1,
            "OrderId": 34,
            "ElectronicInvoiceId": 32,
            "RefundType": 3,
            "Status": 2,
            "RefundAmount": 2900,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-26T02:39:20.671"
        },
        {
            "RefundId": 3,
            "OrderId": 31,
            "ElectronicInvoiceId": 29,
            "RefundType": 3,
            "Status": 2,
            "RefundAmount": 6000,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-26T06:12:30.768"
        },
        {
            "RefundId": 4,
            "OrderId": 33,
            "ElectronicInvoiceId": 0,
            "RefundType": 3,
            "Status": 2,
            "RefundAmount": 2900,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-28T07:42:30.148"
        },
        {
            "RefundId": 5,
            "OrderId": 48,
            "ElectronicInvoiceId": 43,
            "RefundType": 3,
            "Status": 2,
            "RefundAmount": 250,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-28T07:52:53.338"
        },
        {
            "RefundId": 6,
            "OrderId": 15,
            "ElectronicInvoiceId": 0,
            "RefundType": 3,
            "Status": 2,
            "RefundAmount": 15000,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-28T09:37:28.763"
        },
        {
            "RefundId": 7,
            "OrderId": 21,
            "ElectronicInvoiceId": 17,
            "RefundType": 1,
            "Status": 2,
            "RefundAmount": 2900,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-28T10:04:46.780"
        },
        {
            "RefundId": 8,
            "OrderId": 49,
            "ElectronicInvoiceId": 45,
            "RefundType": 1,
            "Status": 2,
            "RefundAmount": 2900,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-30T02:32:01.134"
        },
        {
            "RefundId": 9,
            "OrderId": 50,
            "ElectronicInvoiceId": 46,
            "RefundType": 1,
            "Status": 1,
            "RefundAmount": 2900,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-30T02:39:08.977"
        },
        {
            "RefundId": 10,
            "OrderId": 39,
            "ElectronicInvoiceId": 47,
            "RefundType": 2,
            "Status": 2,
            "RefundAmount": 6000,
            "PenaltyAmount": 1200,
            "CreateTime": "2025-06-30T09:12:56.718"
        },
        {
            "RefundId": 11,
            "OrderId": 38,
            "ElectronicInvoiceId": 49,
            "RefundType": 1,
            "Status": 1,
            "RefundAmount": 6000,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-30T09:43:39.836"
        },
        {
            "RefundId": 12,
            "OrderId": 37,
            "ElectronicInvoiceId": 50,
            "RefundType": 1,
            "Status": 1,
            "RefundAmount": 10000,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-30T09:47:46.130"
        },
        {
            "RefundId": 13,
            "OrderId": 36,
            "ElectronicInvoiceId": 51,
            "RefundType": 2,
            "Status": 1,
            "RefundAmount": 10000,
            "PenaltyAmount": 2000,
            "CreateTime": "2025-06-30T09:48:18.424"
        },
        {
            "RefundId": 14,
            "OrderId": 35,
            "ElectronicInvoiceId": 53,
            "RefundType": 2,
            "Status": 1,
            "RefundAmount": 2900,
            "PenaltyAmount": 580,
            "CreateTime": "2025-06-30T09:59:44.703"
        }
    ]


    mock.onPost("/api/Refund/GetRefund").reply((config) => {
        let {
            Status,
            RefundType,
            SortOrder,
            SortOption,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = refunds.filter(item => {
            const matchStatus = Status === null || item.Status === Number(Status);
            const matchType = RefundType === null || item.RefundType === Number(RefundType);
            return matchStatus && matchType;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "status") {
            field = "Status";
        } else if (SortOption === "createTime") {
            field = "CreateTime";
        }
        else {
            field = "RefundId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const paged = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            RefundList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })
}