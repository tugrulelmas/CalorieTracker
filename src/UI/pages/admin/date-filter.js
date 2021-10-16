import { useState } from 'react';

export default function DateFilter({ onList, loading }) {
    const [dates, setDates] = useState({ from: new Date(new Date().setDate((new Date().getDate()) - 7)), to: new Date() })

    const filter = (factor) => {
        const days = 7 * factor;
        const to = new Date(dates.to.setDate(dates.to.getDate() + days));
        const from = new Date(dates.from.setDate(dates.from.getDate() + days));
        setDates({ from: from, to: to });
        onList(to);
    }

    return (
        <div className="px-4 py-3 mb-8 bg-white rounded-lg shadow-md">
            <div className="">
                <div className="flex justify-center">
                    <div className="mt-4">
                        <button
                            disabled={loading}
                            className={"px-4 py-2 text-sm font-medium leading-5 text-white border bg-purple-600 rounded-lg hover:bg-purple-700 focus:outline-none focus:shadow-outline-purple" + (loading ? ' opacity-50 cursor-not-allowed' : '')}
                            onClick={() => filter(-1)}>
                            &lt;
                    </button>
                    </div>
                    <div className="mt-6 ml-4">
                        <span className="text-gray-700">{dates.from.toLocaleDateString()}</span>
                    </div>
                    <div className="mt-6 ml-4">
                        <span className="text-gray-700">-</span>
                    </div>
                    <div className="mt-6 ml-4">
                        <span className="text-gray-700">{dates.to.toLocaleDateString()}</span>
                    </div>
                    <div className="mt-4 ml-4">
                        <button
                            disabled={loading}
                            className={"px-4 py-2 text-sm font-medium leading-5 text-white border bg-purple-600 rounded-lg hover:bg-purple-700 focus:outline-none focus:shadow-outline-purple" + (loading ? ' opacity-50 cursor-not-allowed' : '')}
                            onClick={() => filter(1)}>
                            &gt;
                    </button>
                    </div>
                </div>
            </div>
        </div>)
}