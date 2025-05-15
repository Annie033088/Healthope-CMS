export default function (mock) {
    let coachs=[
  {
    CoachId: 1,
    Account: "coachAlice",
    Email: "alice@example.com",
    Phone: 912345678,
    Name: "Alice",
    PhotoUrl: "https://example.com/photo1.jpg",
    Introduction: "熱愛健身，擅長塑形。",
    Specialty: "重訓、有氧、TRX、體態雕塑",
    Certification: "ACE私人教練證照、TRX認證",
    Status: true,
    Type: 1,
    ContractStartTime: "2023-03-01",
    ContractEndTime: "2025-03-01",
    CreateTime: "2025-05-15T10:00:00",
    UpdateTime: "2025-05-15T10:00:00"
  },
  {
    Coach: 2,
    Account: "coachBob",
    Email: "bob@example.com",
    Phone: 923456781,
    Name: "Bob",
    Photo: "https://example.com/photo2.jpg",
    Introduction: "注重學術背景的教練。",
    Specialty: "運動科學、體能訓練、姿勢調整",
    Certification: "NSCA認證、運動傷害預防課程",
    Status: false,
    Type: 0,
    ContractStartTime: "2022-07-15",
    ContractEndTime: "2024-07-15",
    CreateTime: "2025-05-15T10:00:00",
    UpdateTime: "2025-05-15T10:00:00"
  },
  {
    Coach: 3,
    Account: "coachCathy",
    Email: "cathy@example.com",
    Phone: 934567892,
    Name: "Cathy",
    Photo: "https://example.com/photo3.jpg",
    Introduction: "專攻女性體態與飲食調整。",
    Specialty: "孕婦運動、飲食控制、塑身",
    Certification: "CPR認證、孕婦運動專業證書",
    Status: true,
    Type: 2,
    ContractStartTime: "2024-01-10",
    ContractEndTime: "2025-12-31",
    CreateTime: "2025-05-15T10:00:00",
    UpdateTime: "2025-05-15T10:00:00"
  },
  {
    Coach: 4,
    Account: "coachDaniel",
    Email: "daniel@example.com",
    Phone: 945678903,
    Name: "Daniel",
    Photo: "https://example.com/photo4.jpg",
    Introduction: "擁有豐富比賽經驗的選手。",
    Specialty: "CrossFit、比賽備賽、爆發力訓練",
    Certification: "CrossFit L1證照、運動營養學證書",
    Status: true,
    Type: 1,
    ContractStartTime: "2021-11-05",
    ContractEndTime: "2026-11-05",
    CreateTime: "2025-05-15T10:00:00",
    UpdateTime: "2025-05-15T10:00:00"
  },
  {
    Coach: 5,
    Account: "coachEmma",
    Email: "emma@example.com",
    Phone: 956789014,
    Name: "Emma",
    Photo: "https://example.com/photo5.jpg",
    Introduction: "親切又有效率的教學風格。",
    Specialty: "初學者教學、塑形、營養諮詢",
    Certification: "體適能C級證照、營養諮詢師",
    Status: false,
    Type: 0,
    ContractStartTime: "2023-08-20",
    ContractEndTime: "2025-08-20",
    CreateTime: "2025-05-15T10:00:00",
    UpdateTime: "2025-05-15T10:00:00"
  }
]

    mock.onPost("/api/Coach/AddCoach").reply(() => {
        // for (let [key, value] of config.data.entries()) {
        //     console.log(key, value);
        // }
        return [200, { ErrorCode: 1 }]
    })
    mock.onPost("/api/Coach/GetCoach").reply((config) => {
        let {
            Status,
            SortOrder,
            SortOption,
            RecordPerPage,
            SearchName,
            SearchPhone,
            Page
        } = JSON.parse(config.data);

        Status = Status === "true" ? true : Status;
        Status = Status === "false" ? false : Status;

        // 1️⃣ 篩選
        let filtered = coachs.filter(item => {
            const matchStatus = Status === null || item.Status === Status;
            const matchName = !SearchName || item.Name.includes(SearchName);
            const matchPhone = !SearchPhone || item.Phone.slice(-3) === SearchPhone;
            return matchStatus && matchName && matchPhone;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "name") {
            field = "Name";
        } else if (SortOption === "status") {
            field = "Status";
        }
        else if (SortOption === "contractEndTime") {
            field = "ContractEndTime"
        }else {
            field = "CoachId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (SortOption === 'status') {
                aVal = aVal ? 1 : 0;
                bVal = bVal ? 1 : 0;
            }

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const pageCoachData = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            CoachList: pageCoachData,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })
}