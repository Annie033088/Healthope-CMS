export default function (mock) {
    let revenueExpenseReports = [
        {
            MembershipRevenue: 2500,
            PersonalTrainingRevenue: 4800,
            SingleEntryRevenue: 200,
            TotalRevenue: 7500,
            RefundExpense: 500,
            PenaltyIncome: 100,
            NetRevenue: 7100,
            Year: 2025,
            Month: 5,
            Day: 5,
        },
        {
            MembershipRevenue: 3200,
            PersonalTrainingRevenue: 5200,
            SingleEntryRevenue: 400,
            TotalRevenue: 8800,
            RefundExpense: 600,
            PenaltyIncome: 300,
            NetRevenue: 8500,
            Year: 2025,
            Month: 5,
            Day: 17,
        },
        {
            MembershipRevenue: 2800,
            PersonalTrainingRevenue: 6100,
            SingleEntryRevenue: 250,
            TotalRevenue: 9150,
            RefundExpense: 750,
            PenaltyIncome: 150,
            NetRevenue: 8550,
            Year: 2025,
            Month: 5,
            Day: 27,
        }, {
            MembershipRevenue: 2900,
            PersonalTrainingRevenue: 5700,
            SingleEntryRevenue: 300,
            TotalRevenue: 8900,
            RefundExpense: 900,
            PenaltyIncome: 250,
            NetRevenue: 8250,
            Year: 2025,
            Month: 6,
            Day: 2,
        },
        {
            MembershipRevenue: 3100,
            PersonalTrainingRevenue: 6500,
            SingleEntryRevenue: 350,
            TotalRevenue: 9950,
            RefundExpense: 600,
            PenaltyIncome: 180,
            NetRevenue: 9530,
            Year: 2025,
            Month: 6,
            Day: 15,
        },
        {
            MembershipRevenue: 2700,
            PersonalTrainingRevenue: 4800,
            SingleEntryRevenue: 180,
            TotalRevenue: 7680,
            RefundExpense: 400,
            PenaltyIncome: 100,
            NetRevenue: 7380,
            Year: 2025,
            Month: 6,
            Day: 26,
        },
        {
            MembershipRevenue: 3000,
            PersonalTrainingRevenue: 6000,
            SingleEntryRevenue: 300,
            TotalRevenue: 9300,
            RefundExpense: 1200,
            PenaltyIncome: 200,
            NetRevenue: 8300,
            Year: 2025,
            Month: 7,
            Day: 1,
        },
        {
            MembershipRevenue: 2800,
            PersonalTrainingRevenue: 6200,
            SingleEntryRevenue: 220,
            TotalRevenue: 9220,
            RefundExpense: 800,
            PenaltyIncome: 120,
            NetRevenue: 8540,
            Year: 2025,
            Month: 7,
            Day: 5,
        },
        {
            MembershipRevenue: 3500,
            PersonalTrainingRevenue: 7000,
            SingleEntryRevenue: 500,
            TotalRevenue: 11000,
            RefundExpense: 900,
            PenaltyIncome: 300,
            NetRevenue: 10400,
            Year: 2025,
            Month: 7,
            Day: 9,
        },
    ]

    mock.onPost("/api/Report/GetRevenueExpenseReport").reply(config => {
        let {
            Year,
            Month
        } = JSON.parse(config.data);
        let reportTarget;

        // 月報表
        if (Month) {
            reportTarget = revenueExpenseReports.filter(report =>
                report.Year === Number(Year) && report.Month === Number(Month));
        }
        // 年報表
        else {
            reportTarget = revenueExpenseReports.filter(report =>
                report.Year === Number(Year));
        }

        return [200, { ErrorCode: 1, ApiDataObject: reportTarget }]
    })
}