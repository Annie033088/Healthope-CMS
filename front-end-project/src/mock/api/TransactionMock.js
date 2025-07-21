export default function (mock) {
    const transactions = [
        {
            "TransactionId": 0,
            "OrderId": 2,
            "Method": 1,
            "Amount": 2900,
            "Status": 2,
            "Time": "2025-06-17 08:10:51.105",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 1,
            MemberName: "王小明",
            MemberPhone: 912345678,
        },
        {
            "TransactionId": 0,
            "OrderId": 4,
            "Method": 1,
            "Amount": 10000,
            "Status": 2,
            "Time": "2025-06-16 06:23:52.272",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 2,
            MemberName: "陳美麗",
            MemberPhone: 911223344,
        },
        {
            "TransactionId": 0,
            "OrderId": 5,
            "Method": 1,
            "Amount": 2900,
            "Status": 2,
            "Time": "2025-06-16 06:48:35.003",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 2,
            MemberName: "陳美麗",
            MemberPhone: 911223344,
        },
        {
            "TransactionId": 0,
            "OrderId": 6,
            "Method": 1,
            "Amount": 10000,
            "Status": 2,
            "Time": "2025-06-16 07:00:15.739",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 3,
            MemberName: "李大仁",
            MemberPhone: 913334455,
        },
        {
            "TransactionId": 0,
            "OrderId": 7,
            "Method": 1,
            "Amount": 2900,
            "Status": 2,
            "Time": "2025-06-16 07:02:05.876",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 4,
            MemberName: "張小華",
            MemberPhone: 914556677,
        },
        {
            "TransactionId": 0,
            "OrderId": 8,
            "Method": 1,
            "Amount": 15000,
            "Status": 2,
            "Time": "2025-06-16 07:10:48.956",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 5,
            MemberName: "林阿忠",
            MemberPhone: 915667788,
        },
        {
            "TransactionId": 0,
            "OrderId": 9,
            "Method": 1,
            "Amount": 15000,
            "Status": 2,
            "Time": "2025-06-16 07:24:11.301",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 6,
            MemberName: "周玉芬",
            MemberPhone: 916778899,
        },
        {
            "TransactionId": 8,
            "OrderId": 21,
            "Method": 2,
            "Amount": 2900,
            "Status": 2,
            "Time": "2025-06-20 01:41:49.373",
            "AuthCode": "B42X9",
            "CardLastFour": "6062",
            "CardType": "VISA",
            "GatewayTransactionId": "dbe7330e-5011-4452-a29a-bd2df4198e06",
            MemberId: 7,
            MemberName: "鄭家豪",
            MemberPhone: 917889900,
        },
        {
            "TransactionId": 9,
            "OrderId": 22,
            "Method": 2,
            "Amount": 15000,
            "Status": 2,
            "Time": "2025-06-20 01:50:01.676",
            "AuthCode": "B42X9",
            "CardLastFour": "6062",
            "CardType": "VISA",
            "GatewayTransactionId": "5e44824b-16e8-4393-9166-00a878e06309",
            MemberId: 10,
            MemberName: "賴佩芬",
            MemberPhone: 920112233,
        },
        {
            "TransactionId": 10,
            "OrderId": 28,
            "Method": 2,
            "Amount": 250,
            "Status": 2,
            "Time": "2025-06-23 05:47:37.708",
            "AuthCode": "B42X9",
            "CardLastFour": "6062",
            "CardType": "VISA",
            "GatewayTransactionId": "9cebbef4-5fc1-46ae-892a-f90fb4a636ed",
            MemberId: 9,
            MemberName: "曾明志",
            MemberPhone: 919101112,
        },
        {
            "TransactionId": 11,
            "OrderId": 32,
            "Method": 2,
            "Amount": 6000,
            "Status": 2,
            "Time": "2025-06-23 09:39:24.098",
            "AuthCode": "B42X9",
            "CardLastFour": "6062",
            "CardType": "VISA",
            "GatewayTransactionId": "0d617dfa-751f-4af8-a83b-7c3e7b71c28c",
            MemberId: 8,
            MemberName: "何玉清",
            MemberPhone: 918990011,
        },
        {
            "TransactionId": 12,
            "OrderId": 41,
            "Method": 2,
            "Amount": 2900,
            "Status": 1,
            "Time": "2025-06-25 03:30:08.389",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 7,
            MemberName: "鄭家豪",
            MemberPhone: 917889900,
        },
        {
            "TransactionId": 18,
            "OrderId": 47,
            "Method": 2,
            "Amount": 2900,
            "Status": 3,
            "Time": "2025-06-25 06:40:04.352",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 5,
            MemberName: "林阿忠",
            MemberPhone: 915667788,
        },
        {
            "TransactionId": 19,
            "OrderId": 49,
            "Method": 2,
            "Amount": 2900,
            "Status": 3,
            "Time": "2025-06-30 01:59:41.124",
            "AuthCode": null,
            "CardLastFour": null,
            "CardType": null,
            "GatewayTransactionId": null,
            MemberId: 4,
            MemberName: "張小華",
            MemberPhone: 914556677,
        }
    ]

    mock.onPost("/api/Transaction/GetTransaction").reply((config) => {
        let {
            Status,
            Method,
            SortOrder,
            SortOption,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = transactions.filter(item => {
            const matchStatus = Status === null || item.Status === Number(Status);
            const matchMethod = Method === null || item.Method === Number(Method);
            return matchStatus && matchMethod;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "amount") {
            field = "Amount";
        } else if (SortOption === "time") {
            field = "Time";
        }
        else {
            field = "OrderId"
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
            TransactionList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    }),
        mock.onPost("/api/Transaction/GetCreditCardCashFlowData").reply((config) => {
            let { TransactionId } = JSON.parse(config.data);
            let transaction = transactions.find((transaction) => transaction.TransactionId === TransactionId)
            let ApiDataObject = { GatewayTransactionId: transaction.GatewayTransactionId, AuthCode: transaction.AuthCode }

            return [200, { ErrorCode: 1, ApiDataObject }]
        })
}