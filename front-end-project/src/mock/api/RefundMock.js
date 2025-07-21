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
            "CreateTime": "2025-06-26T02:39:20.671",
            MemberId: 1,
            MemberName: "王小明",
            MemberPhone: 912345678,
        },
        {
            "RefundId": 3,
            "OrderId": 31,
            "ElectronicInvoiceId": 29,
            "RefundType": 3,
            "Status": 2,
            "RefundAmount": 6000,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-26T06:12:30.768",
            MemberId: 2,
            MemberName: "陳美麗",
            MemberPhone: 911223344,
        },
        {
            "RefundId": 4,
            "OrderId": 33,
            "ElectronicInvoiceId": 0,
            "RefundType": 3,
            "Status": 2,
            "RefundAmount": 2900,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-28T07:42:30.148",
            MemberId: 3,
            MemberName: "李大仁",
            MemberPhone: 913334455,
        },
        {
            "RefundId": 5,
            "OrderId": 48,
            "ElectronicInvoiceId": 43,
            "RefundType": 3,
            "Status": 2,
            "RefundAmount": 250,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-28T07:52:53.338",
            MemberId: 4,
            MemberName: "張小華",
            MemberPhone: 914556677,
        },
        {
            "RefundId": 6,
            "OrderId": 15,
            "ElectronicInvoiceId": 0,
            "RefundType": 3,
            "Status": 2,
            "RefundAmount": 15000,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-28T09:37:28.763",
            MemberId: 5,
            MemberName: "林阿忠",
            MemberPhone: 915667788,
        },
        {
            "RefundId": 7,
            "OrderId": 21,
            "ElectronicInvoiceId": 17,
            "RefundType": 1,
            "Status": 2,
            "RefundAmount": 2900,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-28T10:04:46.780",
            MemberId: 6,
            MemberName: "周玉芬",
            MemberPhone: 916778899,
        },
        {
            "RefundId": 8,
            "OrderId": 49,
            "ElectronicInvoiceId": 45,
            "RefundType": 1,
            "Status": 2,
            "RefundAmount": 2900,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-30T02:32:01.134",
            MemberId: 7,
            MemberName: "鄭家豪",
            MemberPhone: 917889900,
        },
        {
            "RefundId": 9,
            "OrderId": 50,
            "ElectronicInvoiceId": 46,
            "RefundType": 1,
            "Status": 1,
            "RefundAmount": 2900,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-30T02:39:08.977",
            MemberId: 8,
            MemberName: "何玉清",
            MemberPhone: 918990011,
        },
        {
            "RefundId": 10,
            "OrderId": 39,
            "ElectronicInvoiceId": 47,
            "RefundType": 2,
            "Status": 2,
            "RefundAmount": 6000,
            "PenaltyAmount": 1200,
            "CreateTime": "2025-06-30T09:12:56.718",
            MemberId: 9,
            MemberName: "曾明志",
            MemberPhone: 919101112,
        },
        {
            "RefundId": 11,
            "OrderId": 38,
            "ElectronicInvoiceId": 49,
            "RefundType": 1,
            "Status": 1,
            "RefundAmount": 6000,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-30T09:43:39.836",
            MemberId: 10,
            MemberName: "賴佩芬",
            MemberPhone: 920112233,
        },
        {
            "RefundId": 12,
            "OrderId": 37,
            "ElectronicInvoiceId": 50,
            "RefundType": 1,
            "Status": 1,
            "RefundAmount": 10000,
            "PenaltyAmount": 0,
            "CreateTime": "2025-06-30T09:47:46.130",
            MemberId: 11,
            MemberName: "陳小明",
            MemberPhone: 920222233,
        },
        {
            "RefundId": 13,
            "OrderId": 36,
            "ElectronicInvoiceId": 51,
            "RefundType": 2,
            "Status": 1,
            "RefundAmount": 10000,
            "PenaltyAmount": 2000,
            "CreateTime": "2025-06-30T09:48:18.424",
            MemberId: 9,
            MemberName: "曾明志",
            MemberPhone: 919101112,
        },
        {
            "RefundId": 14,
            "OrderId": 35,
            "ElectronicInvoiceId": 53,
            "RefundType": 2,
            "Status": 1,
            "RefundAmount": 2900,
            "PenaltyAmount": 580,
            "CreateTime": "2025-06-30T09:59:44.703",
            MemberId: 8,
            MemberName: "何玉清",
            MemberPhone: 918990011,
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