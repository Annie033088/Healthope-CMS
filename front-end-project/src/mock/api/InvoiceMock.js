export default function (mock) {
    const invoiceTracks = [
        {
            InvoiceTrackNumberId: 1,
            TrackPrefix: "AB",
            StartNumber: 1,
            EndNumber: 9999,
            CurrentNumber: 2345,
            InvoicePeriod: 1143, // 民國114年第三期
            Status: 2, // Active
            CreateTime: "2025-06-01T10:15:30.123"
        },
        {
            InvoiceTrackNumberId: 2,
            TrackPrefix: "CD",
            StartNumber: 10000,
            EndNumber: 19999,
            CurrentNumber: 19999,
            InvoicePeriod: 1142, // 民國114年第二期
            Status: 4, // Closed
            CreateTime: "2025-04-01T09:00:00.000"
        },
        {
            InvoiceTrackNumberId: 3,
            TrackPrefix: "EF",
            StartNumber: 20000,
            EndNumber: 29999,
            CurrentNumber: 20000,
            InvoicePeriod: 1144,
            Status: 1,
            CreateTime: "2025-07-01T14:45:20.567"
        },
        {
            InvoiceTrackNumberId: 4,
            TrackPrefix: "EF",
            StartNumber: 1,
            EndNumber: 29999,
            CurrentNumber: 1,
            InvoicePeriod: 1142,
            Status: 1,
            CreateTime: "2025-07-01T14:45:20.567"
        }
    ];

    const electronicInvoices = [
        {
            "ElectronicInvoiceId": 33,
            "OrderId": 35,
            "InvoiceNumber": "AC00000030",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "5267",
            "Buyer": "00000000",
            "TotalAmount": 2900,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-24T08:00:05.123"
        },
        {
            "ElectronicInvoiceId": 38,
            "OrderId": 13,
            "InvoiceNumber": "AC00000035",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "4172",
            "Buyer": "00000000",
            "TotalAmount": 15000,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-24T09:08:36.987"
        },
        {
            "ElectronicInvoiceId": 41,
            "OrderId": 40,
            "InvoiceNumber": "AC00000038",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "7799",
            "Buyer": "00000000",
            "TotalAmount": 10000,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-24T09:50:35.265"
        },
        {
            "ElectronicInvoiceId": 47,
            "OrderId": 39,
            "InvoiceNumber": "AC00000044",
            "InvoiceTime": "2025-06-30T09:12:43.000",
            "RandomNumber": "6142",
            "Buyer": "00000000",
            "TotalAmount": 6000,
            "Type": 1,
            "Category": 1,
            "Status": 7,
            "CreateTime": "2025-06-30T09:12:41.777"
        },
        {
            "ElectronicInvoiceId": 11,
            "OrderId": 19,
            "InvoiceNumber": "AC00000010",
            "InvoiceTime": "2025-06-16T08:33:07.000",
            "RandomNumber": "8655",
            "Buyer": "00000000",
            "TotalAmount": 250,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-16T08:33:02.833"
        },
        {
            "ElectronicInvoiceId": 24,
            "OrderId": 6,
            "InvoiceNumber": "AC00000021",
            "InvoiceTime": "2025-06-20T06:58:28.000",
            "RandomNumber": "8980",
            "Buyer": "00000000",
            "TotalAmount": 10000,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-20T06:58:27.384"
        },
        {
            "ElectronicInvoiceId": 19,
            "OrderId": 3,
            "InvoiceNumber": "AC00000059",
            "InvoiceTime": "2025-06-20T06:58:28.000",
            "RandomNumber": "8985",
            "Buyer": "00000000",
            "TotalAmount": 8000,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-20T06:58:27.384"
        },
        {
            "ElectronicInvoiceId": 90,
            "OrderId": 3,
            "InvoiceNumber": "AC00000066",
            "InvoiceTime": "2025-06-25T02:58:28.000",
            "RandomNumber": "8310",
            "Buyer": "00000000",
            "TotalAmount": 1000,
            "Type": 1,
            "Category": 2,
            "Status": 3,
            "CreateTime": "2025-06-20T06:58:27.384"
        },
        {
            "ElectronicInvoiceId": 14,
            "OrderId": 20,
            "InvoiceNumber": "AC00000013",
            "InvoiceTime": "2025-06-19T08:35:21.000",
            "RandomNumber": "7815",
            "Buyer": "00000000",
            "TotalAmount": 250,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-19T08:35:19.439"
        },
        {
            "ElectronicInvoiceId": 52,
            "OrderId": 36,
            "InvoiceNumber": "AC00000049",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "9375",
            "Buyer": "00000000",
            "TotalAmount": 2000,
            "Type": 1,
            "Category": 2,
            "Status": 1,
            "CreateTime": "2025-06-30T09:48:18.424"
        },
        {
            "ElectronicInvoiceId": 6,
            "OrderId": 8,
            "InvoiceNumber": "AC00000005",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "1808",
            "Buyer": "00000000",
            "TotalAmount": 15000,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-16T07:10:48.956"
        },
        {
            "ElectronicInvoiceId": 25,
            "OrderId": 27,
            "InvoiceNumber": "AC00000022",
            "InvoiceTime": "2025-06-23T05:47:04.000",
            "RandomNumber": "7105",
            "Buyer": "00000000",
            "TotalAmount": 15000,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-23T05:47:03.224"
        },
        {
            "ElectronicInvoiceId": 5,
            "OrderId": 7,
            "InvoiceNumber": "AC00000004",
            "InvoiceTime": "2025-06-16T07:02:07.000",
            "RandomNumber": "8376",
            "Buyer": "00000000",
            "TotalAmount": 2900,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-16T07:02:05.876"
        },
        {
            "ElectronicInvoiceId": 29,
            "OrderId": 31,
            "InvoiceNumber": "AC00000026",
            "InvoiceTime": "2025-06-23T09:08:11.000",
            "RandomNumber": "6011",
            "Buyer": "00000000",
            "TotalAmount": 6000,
            "Type": 1,
            "Category": 1,
            "Status": 5,
            "CreateTime": "2025-06-23T09:08:09.476"
        },
        {
            "ElectronicInvoiceId": 18,
            "OrderId": 22,
            "InvoiceNumber": "AC00000015",
            "InvoiceTime": "2025-06-20T01:50:05.000",
            "RandomNumber": "8878",
            "Buyer": "00000000",
            "TotalAmount": 15000,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-20T01:50:05.009"
        },
        {
            "ElectronicInvoiceId": 2,
            "OrderId": 4,
            "InvoiceNumber": "AC00000001",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "5469",
            "Buyer": "00000000",
            "TotalAmount": 10000,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-16T06:23:52.274"
        },
        {
            "ElectronicInvoiceId": 45,
            "OrderId": 49,
            "InvoiceNumber": "AC00000042",
            "InvoiceTime": "2025-06-30T02:17:25.000",
            "RandomNumber": "4273",
            "Buyer": "00000000",
            "TotalAmount": 2900,
            "Type": 1,
            "Category": 1,
            "Status": 7,
            "CreateTime": "2025-06-30T02:17:05.210"
        }
    ]


    mock.onPost("/api/Invoice/AddInvoiceTrackNumber").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Invoice/GetInvoiceTrackNumber").reply((config) => {
        let {
            Status,
            Time,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        const pastTime = 0;

        const now = new Date();
        let taiwanYear = now.getFullYear() - 1911; // 西元轉民國
        let period = Math.floor((now.getMonth() + 1 + 1) / 2); // 兩個月為一期，1~6期
        let nowInvoicePeriod = taiwanYear * 10 + period;

        // 1️⃣ 篩選
        let filtered = invoiceTracks.filter(item => {
            const matchStatus = Status === null || item.Status === Number(Status);
            let matchTime = true;
            if (Time !== null)
                if (Number(Time) == pastTime)
                    matchTime = item.InvoicePeriod < nowInvoicePeriod
                else
                    matchTime = item.InvoicePeriod >= nowInvoicePeriod
            return matchStatus && matchTime;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const paged = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            InvoiceTrackNumberList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/Invoice/DeleteInvoiceTrackNumber").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Invoice/EditInvoiceTrackNumberStatus").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Invoice/PrintInvoice").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Invoice/VoidInvoice").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Invoice/DiscountInvoice").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Invoice/GetInvoice").reply((config) => {
        let {
            Status,
            Category,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);


        // 1️⃣ 篩選
        let filtered = electronicInvoices.filter(item => {
            const matchStatus = Status === null || item.Status === Number(Status);
            const matchCategory = Category === null || item.Category === Number(Category);
            return matchStatus && matchCategory;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const paged = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            InvoiceList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })
}