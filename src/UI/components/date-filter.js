import { useState, useEffect } from 'react';
import moment from 'moment';

export default function DateFilter({ onList, loading }) {
    const [dates, setDates] = useState({ from: moment(new Date()).format('yyyy-MM-DD'), to: moment(new Date()).format('yyyy-MM-DD') });
    const [error, setError] = useState();

    useEffect(() => {
        onList(dates);
    }, []);

    const dateChanged = (e) => {
        setDates({ ...dates, [e.target.name]: e.target.value });
    }

    const filter = async () => {
        if (dates.from && dates.to) {
            const from = new Date(dates.from);
            const to = new Date(dates.to);
            if (from > to) {
                setError('From must be less than To.');
                return;
            }
        }

        setError('');
        onList(dates);
    }

    return (
        <div className="px-4 py-3 mb-8 bg-white rounded-lg shadow-md">
            <div className="flex items-center">
                <label className="text-sm">
                    <span className="text-gray-700">From</span>
                    <input name="from" className="block w-full mt-1 text-sm focus:outline-none focus:shadow-outline-purple form-input"
                        type="date" value={dates.from} onChange={dateChanged} />
                </label>
                <label className="text-sm ml-6">
                    <span className="text-gray-700">To</span>
                    <input name="to" className="block w-full mt-1 text-sm focus:outline-none focus:shadow-outline-purple form-input"
                        type="date" value={dates.to} onChange={dateChanged} />
                </label>
                <div className="mt-6 ml-6">
                    <button
                        disabled={loading}
                        className={"px-4 py-2 text-sm font-medium leading-5 text-white border bg-purple-600 rounded-lg hover:bg-purple-700 focus:outline-none focus:shadow-outline-purple" + (loading ? ' opacity-50 cursor-not-allowed' : '')}
                        onClick={filter}>
                        List
                    </button>
                </div>
                {error && <div className="mt-6 ml-6">
                    <span className="text-red-600">{error}</span>
                </div>
                }
            </div>
        </div>)
}