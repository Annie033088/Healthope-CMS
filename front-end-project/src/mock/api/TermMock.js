
export default function (mock) {
    const terms = [
        {
            TermId: 1,
            Version: 1,
            Name: "教練 - 使用條款",
            DetailContent: "本條款適用於所有教練使用本平台服務的基本規範。",
            Type: 1,
            ApplicableTarget: 2,
            VersionDescription: "初版",
            Status: 1,
            EffectiveTime: "2024-01-01T00:00:00.000Z",
            UpdateTime: "2024-01-01T00:00:00.000Z"
        },
        {
            TermId: 2,
            Version: 2,
            Name: "教練 - 使用條款",
            DetailContent: "更新教練責任條款與違規處置機制。",
            Type: 1,
            ApplicableTarget: 2,
            VersionDescription: "更新內容第 5 條",
            Status: 1,
            EffectiveTime: "2024-03-01T00:00:00.000Z",
            UpdateTime: "2024-03-01T00:00:00.000Z"
        },
        {
            TermId: 3,
            Version: 1,
            Name: "會員 - 隱私政策",
            DetailContent: "本政策說明我們如何收集、使用及保護會員的個人資料。",
            Type: 2,
            ApplicableTarget: 1,
            VersionDescription: "初版",
            Status: 1,
            EffectiveTime: "2024-01-01T00:00:00.000Z",
            UpdateTime: "2024-01-01T00:00:00.000Z"
        },
        {
            TermId: 4,
            Version: 1,
            Name: "所有人 - 退款政策",
            DetailContent: "本退款條款適用於所有付款用戶，說明申請及退款標準。",
            Type: 2,
            ApplicableTarget: 2,
            VersionDescription: "初版",
            Status: 1,
            EffectiveTime: "2024-01-10T00:00:00.000Z",
            UpdateTime: "2024-01-10T00:00:00.000Z"
        },
        {
            TermId: 5,
            Version: 2,
            Name: "所有人 - 退款政策",
            DetailContent: "新增退費處理時效規定。",
            Type: 1,
            ApplicableTarget: 2,
            VersionDescription: "新增第 3 條說明",
            Status: 1,
            EffectiveTime: "2024-04-01T00:00:00.000Z",
            UpdateTime: "2024-04-01T00:00:00.000Z"
        },
        {
            TermId: 6,
            Version: 1,
            Name: "教練 - 教練守則",
            DetailContent: "所有教練需遵守的專業行為規範。",
            Type: 2,
            ApplicableTarget: 1,
            VersionDescription: "初版",
            Status: 1,
            EffectiveTime: "2024-02-01T00:00:00.000Z",
            UpdateTime: "2024-02-01T00:00:00.000Z"
        },
        {
            TermId: 7,
            Version: 1,
            Name: "所有人 - 使用條款",
            DetailContent: "說明使用本平台的基本權利與限制。",
            Type: 1,
            ApplicableTarget: 1,
            VersionDescription: "初版",
            Status: 2,
            EffectiveTime: "2024-01-01T00:00:00.000Z",
            UpdateTime: "2024-01-01T00:00:00.000Z"
        },
        {
            TermId: 8,
            Version: 2,
            Name: "所有人 - 使用條款",
            DetailContent: "新增帳號管理與停權條件。",
            Type: 1,
            ApplicableTarget: 2,
            VersionDescription: "新增第 4 條與第 6 條",
            Status: 1,
            EffectiveTime: "2024-04-10T00:00:00.000Z",
            UpdateTime: "2024-04-10T00:00:00.000Z"
        },
        {
            TermId: 9,
            Version: 1,
            Name: "會員 - 使用條款",
            DetailContent: "專為會員設計之平台使用行為規範。",
            Type: 1,
            ApplicableTarget: 1,
            VersionDescription: "初版",
            Status: 1,
            EffectiveTime: "2024-01-01T00:00:00.000Z",
            UpdateTime: "2024-01-01T00:00:00.000Z"
        },
        {
            TermId: 10,
            Version: 1,
            Name: "教練 - 隱私政策",
            DetailContent: "說明教練個人資料之蒐集與使用範圍。",
            Type: 2,
            ApplicableTarget: 2,
            VersionDescription: "初版",
            Status: 1,
            EffectiveTime: "2024-01-15T00:00:00.000Z",
            UpdateTime: "2024-01-15T00:00:00.000Z"
        }
    ];

    mock.onPost("/api/Term/GetOldTerm").reply((config) => {
        let {
            Type,
            ApplicableTarget
        } = JSON.parse(config.data);

        let filtered = terms.filter(item => {
            const matchType = item.Type === Number(Type);
            const matchTarget = item.ApplicableTarget === Number(ApplicableTarget)
            return matchType && matchTarget;
        });

        if (filtered) {
            return [200, { ErrorCode: 1, ApiDataObject: filtered }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/Term/AddTerm").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Term/GetTerm").reply(config => {
        let {
            Type,
            Status,
            ApplicableTarget,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = terms.filter(item => {
            const matchType = Type === null || item.Type === Number(Type);
            const matchStatus = Status === null || item.Status === Number(Status);
            const matchApplicableTarget = ApplicableTarget === null || item.ApplicableTarget === Number(ApplicableTarget);
            return matchType && matchStatus && matchApplicableTarget;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const paged = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            TermList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/Term/GetTermEditDataById").reply((config) => {
        let termIdDto = JSON.parse(config.data);
        let termTarget = terms.find(term => term.TermId === Number(termIdDto.TermId));

        if (termTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: termTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/Term/EditTerm").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Term/EditTermStatus").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Term/GetTermDetail").reply((config) => {
        let termIdDto = JSON.parse(config.data);
        let termTarget = terms.find(term => term.TermId === Number(termIdDto.TermId));

        if (termTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: termTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/Term/DeleteTerm").reply(() => {
        return [200, { ErrorCode: 1 }]
    })
}