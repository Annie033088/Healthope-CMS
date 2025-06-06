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
}